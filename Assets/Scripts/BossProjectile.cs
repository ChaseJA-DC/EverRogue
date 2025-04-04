using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float damage = 1f;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after 'lifetime' seconds (float is allowed here)
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage((int)damage); // Cast float to int here if needed
            }
            Destroy(gameObject);
        }
    }
}
