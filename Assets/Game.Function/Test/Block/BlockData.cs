using UnityEngine;

[CreateAssetMenu(menuName = "Block/BlockData")]
public class BlockData : ScriptableObject
{
    public Vector2Int[] shapeOffsets; 
    public GameObject blockTilePrefab; 
}
