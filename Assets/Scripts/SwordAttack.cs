using System.Collections;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public float damage = 3;
    public float attackDuration = 0.2f;

    private Animator animator;
    private Transform playerTransform;

    private void Start()
    {
        if (swordCollider == null)
        {
            swordCollider = GetComponent<Collider2D>(); // Auto-assign if not set
        }

        animator = GetComponentInParent<Animator>(); // Get the player's Animator
        playerTransform = GetComponentInParent<Transform>(); // Get player's Transform
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click attack
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (playerTransform == null || animator == null) return;

        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - playerTransform.position).normalized;

        animator.SetFloat("MouseHorizontal", direction.x);
        animator.SetFloat("MouseVertical", direction.y);
        animator.SetTrigger("swordAttack");

        swordCollider.enabled = true;
        StartCoroutine(DisableAttack());
    }

    public void AttackLeft()
    {
        animator.SetFloat("MouseHorizontal", -1);
        animator.SetFloat("MouseVertical", 0);
        animator.SetTrigger("swordAttack");

        swordCollider.enabled = true;
        StartCoroutine(DisableAttack());
    }

    public void AttackRight()
    {
        animator.SetFloat("MouseHorizontal", 1);
        animator.SetFloat("MouseVertical", 0);
        animator.SetTrigger("swordAttack");

        swordCollider.enabled = true;
        StartCoroutine(DisableAttack());
    }

    public void StopAttack()
    {
        animator.ResetTrigger("swordAttack");
        swordCollider.enabled = false;
    }

    public void EndAttack()
    {
        animator.ResetTrigger("swordAttack");
    }

    private IEnumerator DisableAttack()
    {
        yield return new WaitForSeconds(attackDuration);
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
