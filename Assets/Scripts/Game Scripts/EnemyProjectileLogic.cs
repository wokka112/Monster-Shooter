using UnityEngine;

public class EnemyProjectileLogic : MonoBehaviour
{
    public int damage = 1;

    public float impactForce = 5f;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Go through enemies
        if (other.CompareTag("Enemy"))
        {
            return;
        }

        // Damage player
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerController>().HitByEnemyProjectile(damage);
        }

        // Play hit sound for enemy projectile
        audioManager.Play("EnemyProjectileHit");
        Destroy(gameObject);
    }
}
