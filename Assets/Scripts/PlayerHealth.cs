using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("Player took damage! HP: " + currentHealth);

        if (spriteRenderer != null)
            StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player has died!");

        if (animator != null)
        {
            animator.SetTrigger("death");
        }

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<SwordAttack>().enabled = false;
        GetComponent<ShootAttack>().enabled = false;
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.07f);
        spriteRenderer.color = originalColor;
    }
}
