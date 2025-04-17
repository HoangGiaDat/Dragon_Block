using Pooling;
using Singletons;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public ColorPart cannonColor;
    public float range = 5f;
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public LayerMask targetLayer;

    private Transform headTransform;
    private float fireTimer = 0f;

    private void Start()
    {
        headTransform = SingleBehaviour.Of<DragonManager>().bodyParts[0];
    }

    private void FixedUpdate()
    {
        CalculateAction();
    }
    
    private void CalculateAction()
    {
        float dt = Time.deltaTime;
        
        fireTimer -= dt;

        Transform targetToShoot = FindNearestValidTarget();

        if (targetToShoot != null && fireTimer <= 0f)
        {
            Shoot(targetToShoot);
            fireTimer = 1f / fireRate;
        }
    }

    private void Shoot(Transform target)
    {
        GameObject bullet = SingleBehaviour.Of<PoolService>().Spawn(bulletPrefab);
        bullet.transform.position = firePoint.position;
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
        bulletScript.SetColor(cannonColor);
        bulletScript.SetDamage(1);
    }

    private Transform FindNearestValidTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, targetLayer);
        Transform nearest = null;
        float minDistanceToHead = Mathf.Infinity;

        foreach (var hit in hits)
        {
            DragonPart part = hit.GetComponent<DragonPart>();
            if (part != null && part.partColor == cannonColor)
            {
                float distToHead = Vector3.Distance(part.transform.position, headTransform.position);
                if (distToHead < minDistanceToHead)
                {
                    minDistanceToHead = distToHead;
                    nearest = part.transform;
                }
            }
        }

        return nearest;
    }
}

public enum ColorPart
{
    None,
    Red,
    Green,
    Pink,
}