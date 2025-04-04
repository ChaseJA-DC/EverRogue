using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 3f;
    public float moveSpeed = 2f;
    public int damage = 1;
    public GameObject dropPrefab;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform player;

    private bool isFlashing = false;
    private bool isStunned = false; // ✅ NEW

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (player != null && !isStunned)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log($"Enemy hit for {damageAmount} damage!");
        StartCoroutine(FlashWhite());

        if (health <= 0)
        {
            Defeated();
        }
    }

    public void ApplyHitstun(float duration, Vector2 knockbackDir)
    {
        if (!isStunned)
            StartCoroutine(Hitstun(duration, knockbackDir));
    }


    private IEnumerator Hitstun(float duration, Vector2 knockbackDir)
    {
        isStunned = true;

        // Apply knockback
        rb.velocity = knockbackDir * 1f;

        yield return new WaitForSeconds(duration);

        rb.velocity = Vector2.zero;
        isStunned = false;
    }

    private IEnumerator FlashWhite()
    {
        if (isFlashing) yield break;
        isFlashing = true;

        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = originalColor;

        isFlashing = false;
    }

    private void Defeated()
    {
        rb.velocity = Vector2.zero;
        rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;

        if (dropPrefab != null)
        {
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }
}
