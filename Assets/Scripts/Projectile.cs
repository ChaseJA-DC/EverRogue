using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifeTime = 2f;
    public float damage = 5f;
    public float knockbackForce = 0.5f;
    private Vector2 direction;
    private bool hasHit = false;
    public bool isFromPlayer = true;

    public void Initialize(Vector2 shootDirection)
    {
        direction = shootDirection.normalized;
        Destroy(gameObject, lifeTime);

        // Rotate sprite to face firing direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f); // +90 to align bottom
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Vector2 knockbackDir = direction;
            enemy.ApplyHitstun(0.3f, knockbackDir * knockbackForce);
            hasHit = true;
            Destroy(gameObject);
        }
    }
}
