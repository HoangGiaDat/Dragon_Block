using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x, y;
    public bool occupied;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
        SetOccupied(false);
    }

    public void SetOccupied(bool value)
    {
        occupied = value;
        _sprite.color = value ? Color.gray : Color.white;
    }

    public void Clear()
    {
        SetOccupied(false);
    }
}
