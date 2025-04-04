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
        // Simple horizontal movement (ping-pong)
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveRange * 2) - moveRange;
        transform.position = startPosition + new Vector3(offset, 0, 0);

        // Fire timer
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
            Vector2.down,                    // middle
            (Vector2.down + Vector2.left).normalized,  // left
            (Vector2.down + Vector2.right).normalized  // right
        };

        foreach (Vector2 dir in directions)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = dir * 5f; // Adjust speed as needed
        }
    }
}
