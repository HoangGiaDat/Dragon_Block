using UnityEngine;
using System.Collections.Generic;
using Singletons;
using System;

public class Block : MonoBehaviour
{
    public BlockData blockData;
    private List<Transform> tiles = new List<Transform>();
    private Vector3 dragOffset;
    public Action<Block> onBlockPlaced;

    private void Start()
    {
        foreach (var offset in blockData.shapeOffsets)
        {
            GameObject tile = Instantiate(blockData.blockTilePrefab, transform);
            tile.transform.localPosition = new Vector3(offset.x, offset.y, 0);
            tiles.Add(tile.transform);
        }
    }

    private void OnMouseDown()
    {
        dragOffset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + dragOffset;
    }

    private void OnMouseUp()
    {
        TryPlaceOnGrid();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    private void TryPlaceOnGrid()
    {
        Vector3 nearestCellPos = FindNearestCellWorldPos();
        Vector2Int originGridPos = WorldToGrid(nearestCellPos);

        if (CanPlaceAt(originGridPos))
        {
            PlaceBlock(originGridPos);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Không thể đặt block ở đây!");
        }
    }

    private Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / SingleBehaviour.Of<GridManager>().cellSpacing);
        int y = Mathf.RoundToInt(worldPos.y / SingleBehaviour.Of<GridManager>().cellSpacing);
        return new Vector2Int(x, y);
    }

    private Vector3 FindNearestCellWorldPos()
    {
        Vector3 center = transform.position;
        float spacing = SingleBehaviour.Of<GridManager>().cellSpacing;

        int x = Mathf.RoundToInt(center.x / spacing);
        int y = Mathf.RoundToInt(center.y / spacing);

        return new Vector3(x * spacing, y * spacing, 0);
    }

    private bool CanPlaceAt(Vector2Int origin)
    {
        foreach (var offset in blockData.shapeOffsets)
        {
            int x = origin.x + offset.x;
            int y = origin.y + offset.y;

            if (x < 0 || y < 0 || x >= 8 || y >= 8) 
                return false;
            
            if (!SingleBehaviour.Of<GridManager>().IsCellAvailable(x, y)) 
                return false;
        }
        return true;
    }

    private void PlaceBlock(Vector2Int origin)
    {
        foreach (var offset in blockData.shapeOffsets)
        {
            int x = origin.x + offset.x;
            int y = origin.y + offset.y;
            SingleBehaviour.Of<GridManager>().SetCellOccupied(x, y, true);
        }
        
        for (int i = 0; i < 8; i++)
        {
            if (SingleBehaviour.Of<GridManager>().CheckFullRow(i)) 
                SingleBehaviour.Of<GridManager>().ClearRow(i);
            
            if (SingleBehaviour.Of<GridManager>().CheckFullColumn(i))
                SingleBehaviour.Of<GridManager>().ClearColumn(i);
        }
        
        onBlockPlaced?.Invoke(this);
        Destroy(gameObject);
    }
}
