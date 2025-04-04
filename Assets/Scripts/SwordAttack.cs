using System.Collections;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject swordHitboxPrefab;      // Prefab to spawn
    public float damage = 3f;
    public float attackDuration = 0.2f;
    public float hitstunDuration = 0.3f;
    public Transform attackPivot;             // Where to rotate around (player feet)

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();

        if (swordHitboxPrefab == null)
            Debug.LogError("Sword hitbox prefab not assigned!");

        if (attackPivot == null)
            Debug.LogError("Attack pivot not assigned!");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (attackPivot == null || animator == null || swordHitboxPrefab == null) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - attackPivot.position).normalized;

        animator.SetFloat("MouseHorizontal", direction.x);
        animator.SetFloat("MouseVertical", direction.y);
        animator.SetTrigger("swordAttack");

        float swordDistance = 0.5f;
        Vector3 spawnPos = attackPivot.position + (Vector3)(direction * swordDistance);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject hitbox = Instantiate(swordHitboxPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
        SwordHitbox hitboxScript = hitbox.GetComponent<SwordHitbox>();

        if (hitboxScript != null)
        {
            hitboxScript.Setup(damage, hitstunDuration);
        }
        else
        {
            Debug.LogError("SwordHitbox script missing on prefab!");
        }

        Destroy(hitbox, attackDuration);
    }


    public void EndAttack()
    {
        animator.ResetTrigger("swordAttack");
    }
}
