using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [Header("Boss Behavior")]
    public float moveRange = 3f;
    public float moveSpeed = 2f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireInterval = 2f;

    [Header("Boss Health")]
    public float maxHealth = 20f;
    public float currentHealth;

    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    private Color originalColor;

    private Vector3 startPosition;
    private float fireTimer;
    public float deathDelay = 3f; 

    private void Start()
    {
        startPosition = transform.position;
        currentHealth = maxHealth;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    }

    private void Update()
    {
        // Ping-pong horizontal movement
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveRange * 2) - moveRange;
        transform.position = startPosition + new Vector3(offset, 0, 0);

        // Fire projectiles on interval
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            FireProjectiles();
            fireTimer = 0f;
        }
    }

    private void FireProjectiles()
    {
        Vector2[] directions = {
            Vector2.down,
            (Vector2.down + Vector2.left).normalized,
            (Vector2.down + Vector2.right).normalized
        };

        foreach (Vector2 dir in directions)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            proj.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);

            BossProjectile bp = proj.GetComponent<BossProjectile>();
            if (bp != null)
            {
                bp.direction = dir;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Boss hit! HP: " + currentHealth);

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.07f);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        Debug.Log("Boss defeated!");
        StartCoroutine(LoadSceneAfterDelay());
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile != null && projectile.isFromPlayer)
        {
            TakeDamage(projectile.damage);
            Destroy(other.gameObject);
            return;
        }

        SwordHitbox sword = other.GetComponent<SwordHitbox>();
        if (sword != null)
        {
            TakeDamage(sword.GetDamage());
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(deathDelay);
        SceneManager.LoadScene(5);
    }
}
