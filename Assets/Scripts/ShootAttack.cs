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
    public Transform firePoint;
    public float projectileSpeed = 5f;
    private bool projectileUnlocked = false;
    private bool canAttack = true;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        playerTransform = GetComponentInParent<Transform>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && projectileUnlocked && canAttack)
        {
            Attack();
        }
    }

    public void EnableProjectile()
    {
        projectileUnlocked = true;
    }

    private void Attack()
    {
        if (playerTransform == null || animator == null || playerMovement == null) return;

        canAttack = false;

        Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (worldMousePos - (Vector2)playerTransform.position).normalized;

        animator.SetFloat("MouseHorizontal", direction.x);
        animator.SetFloat("MouseVertical", direction.y);
        animator.SetTrigger("shootAttack");

        playerMovement.SetTemporaryMoveSpeed(slowSpeed, attackDuration);

        float fireDistance = 0.3f;
        firePoint.position = playerTransform.position + (Vector3)(direction * fireDistance);
        FireProjectile(direction);

        StartCoroutine(ResetAttackCooldown());
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

    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackDuration);
        canAttack = true;
    }

    public void EndAttack()
    {
        animator.ResetTrigger("shootAttack");
    }
}
