using System;
using UnityEngine;
using System.Collections.Generic;
using Singletons;
using Random = UnityEngine.Random;

public class BlockContainer : MonoBehaviour
{
    public Transform[] spawnPoints;
    private List<Block> currentBlocks = new List<Block>();
    public BlockData[] allBlocks;

    private void Start()
    {
        SpawnBlocks();
    }

    public void SpawnBlocks()
    {
        ClearOldBlocks();

        for (int i = 0; i < 3; i++)
        {
            BlockData data = allBlocks[Random.Range(0, allBlocks.Length)];

            GameObject go = new GameObject("Block");
            go.transform.position = spawnPoints[i].position;
            go.transform.SetParent(transform);

            Block block = go.AddComponent<Block>();
            block.blockData = data;
            block.onBlockPlaced = OnBlockPlaced;
            BoxCollider2D boxCollider2D = go.AddComponent<BoxCollider2D>();

            currentBlocks.Add(block);
        }
    }

    private void ClearOldBlocks()
    {
        foreach (var b in currentBlocks)
        {
            if (b != null) Destroy(b.gameObject);
        }
        currentBlocks.Clear();
    }

    private void OnBlockPlaced(Block block)
    {
        currentBlocks.Remove(block);

        if (currentBlocks.Count == 0)
        {
            Invoke(nameof(SpawnBlocks), 0.5f);
        }
    }

    private void CheckGameOver()
    {
        foreach (var block in currentBlocks)
        {
            if (CanBlockBePlaced(block.blockData))
                return;
        }

        Debug.Log("GAME OVER");
    }

    private bool CanBlockBePlaced(BlockData blockData)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Vector2Int origin = new Vector2Int(x, y);
                bool canPlace = true;

                foreach (var offset in blockData.shapeOffsets)
                {
                    int px = origin.x + offset.x;
                    int py = origin.y + offset.y;

                    if (px < 0 || py < 0 || px >= 8 || py >= 8 ||
                        !SingleBehaviour.Of<GridManager>().IsCellAvailable(px, py))
                    {
                        canPlace = false;
                        break;
                    }
                }

                if (canPlace) 
                    return true;
            }
        }
        return false;
    }
}
