using Pooling;
using Singletons;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public SpriteRenderer sprite;
    public float speed = 10f;
    private Transform target;

    private int _damage;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetColor(ColorPart colorPart)
    {
        sprite.color = GameDragonHelper.SetColor(colorPart);
    }
    
    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    void Update()
    {
        if (target == null)
        {
            SingleBehaviour.Of<PoolService>().DeSpawn(gameObject);
            return;
        }

        var transform1 = transform;
        var position = transform1.position;
        Vector3 dir = (target.position - position).normalized;
        position += dir * speed * Time.deltaTime;
        transform1.position = position;
        transform1.up = dir;

        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            target.GetComponent<DragonPart>()?.TakeDamage(_damage);
            SingleBehaviour.Of<PoolService>().DeSpawn(gameObject);
        }
    }
}