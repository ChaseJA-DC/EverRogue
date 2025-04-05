using UnityEngine;

public class BossController : MonoBehaviour
{
    public float moveRange = 3f;
    public float moveSpeed = 2f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireInterval = 2f;

    private Vector3 startPosition;
    private float fireTimer;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveRange * 2) - moveRange;
        transform.position = startPosition + new Vector3(offset, 0, 0);

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            FireProjectiles();
            fireTimer = 0f;
        }
    }

    void FireProjectiles()
    {
        Vector2[] directions = {
            Vector2.down,
            (Vector2.down + Vector2.left).normalized,
            (Vector2.down + Vector2.right).normalized
        };

        foreach (Vector2 dir in directions)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            // Rotate to match direction
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            proj.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);

            // Tell the projectile it's from the boss
            BossProjectile bp = proj.GetComponent<BossProjectile>();
            if (bp != null)
            {
                bp.direction = dir;
            }
        }
    }
}
