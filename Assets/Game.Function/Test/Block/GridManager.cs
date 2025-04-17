using Pooling;
using Singletons;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    public GameObject cellPrefab;
    public Transform gridParent;
    public float cellSpacing = 1.1f;

    [HideInInspector]
    public Cell[,] grid;

    private void Start()
    {
        grid = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * cellSpacing, y * cellSpacing, 0);
                GameObject cellObj  = SingleBehaviour.Of<PoolService>().Spawn(cellPrefab);
                cellPrefab.transform.position = Vector3.zero;
                cellPrefab.transform.position = pos;
                cellObj.transform.SetParent(gridParent);
                Cell cell = cellObj.GetComponent<Cell>();
                cell.Init(x, y);
                grid[x, y] = cell;
            }
        }
    }

    public bool IsCellAvailable(int x, int y)
    {
        return !grid[x, y].occupied;
    }

    public void SetCellOccupied(int x, int y, bool occupied)
    {
        grid[x, y].SetOccupied(occupied);
    }

    public void ClearRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            grid[x, y].Clear();
        }
    }

    public void ClearColumn(int x)
    {
        for (int y = 0; y < height; y++)
        {
            grid[x, y].Clear();
        }
    }

    public bool CheckFullRow(int y)
    {
        for (int x = 0; x < width; x++)
            if (!grid[x, y].occupied) return false;
        return true;
    }

    public bool CheckFullColumn(int x)
    {
        for (int y = 0; y < height; y++)
            if (!grid[x, y].occupied) return false;
        return true;
    }
}
