using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    private float damage;
    private float hitstunDuration;
    private HashSet<Enemy> enemiesHit = new HashSet<Enemy>();

    public void Setup(float dmg, float stunTime)
    {
        damage = dmg;
        hitstunDuration = stunTime;
    }

    public void ResetHitbox()
    {
        enemiesHit.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !enemiesHit.Contains(enemy))
            {
                enemy.TakeDamage(damage);
                Vector2 knockbackDir = (enemy.transform.position - transform.position).normalized;
                enemy.ApplyHitstun(hitstunDuration, knockbackDir);
                enemiesHit.Add(enemy);
            }
        }
    }
}
