using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float damage = 1f;
    public float speed = 3f;
    public float lifetime = 2f;
    public Vector2 direction;
    private bool hasHit = false;

    void Start()
    {
        // Rotate sprite to match direction
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        }

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += (Vector3)(direction.normalized * Time.deltaTime * speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage((int)damage);
            }

            hasHit = true;
            Destroy(gameObject);
        }
    }
}
