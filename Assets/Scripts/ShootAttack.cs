using System.Collections;
using UnityEngine;

public class ShootAttack : MonoBehaviour
{
    public float damage = 3;
    public float attackDuration = 0.7f;
    public float slowSpeed = 0.01f;
    private Animator animator;
    private Transform playerTransform;
    private PlayerMovement playerMovement;
    public GameObject projectilePrefab;
    public Transform firePoint; // Shoot origin point
    public float projectileSpeed = 5f;


    private void Start()
    {
        animator = GetComponentInParent<Animator>(); // Get the player's Animator
        playerTransform = GetComponentInParent<Transform>(); // Get player's Transform
        playerMovement = GetComponentInParent<PlayerMovement>(); // Get player's Movement
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click attack
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (playerTransform == null || animator == null || playerMovement == null) return;

        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - playerTransform.position).normalized;

        animator.SetFloat("MouseHorizontal", direction.x);
        animator.SetFloat("MouseVertical", direction.y);
        animator.SetTrigger("shootAttack");
        
        playerMovement.SetTemporaryMoveSpeed(slowSpeed, attackDuration); // Slow movement

        
        float fireDistance = 0.3f;
        firePoint.position = playerTransform.position + (Vector3)(direction * fireDistance);
        FireProjectile(direction);
    }

    private void FireProjectile(Vector2 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(direction);
        }
    }

    public void EndAttack()
    {
        animator.ResetTrigger("shootAttack");
    }
}
