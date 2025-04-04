using UnityEngine;

public class ProjectilePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShootAttack shootAttack = other.GetComponent<ShootAttack>();
            if (shootAttack != null)
            {
                shootAttack.EnableProjectile();
            }

            Destroy(gameObject);
        }
    }
}
