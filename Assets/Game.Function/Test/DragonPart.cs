using Singletons;
using UnityEngine;

public class DragonPart : MonoBehaviour
{
    public SpriteRenderer sprite;
    public ColorPart partColor;
    public int health = 3;
    public bool IsDead;

    private void Start()
    {
        sprite.color = GameDragonHelper.SetColor(partColor);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            IsDead = true;
            SingleBehaviour.Of<DragonManager>().RemovePart(transform);
        }
    }
}