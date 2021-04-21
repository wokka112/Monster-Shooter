using UnityEngine;

public class FriendlyProjectileLogic : MonoBehaviour
{
    public float impactForce = 5f;

    private int projectileDmg = 1;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();

            // If enemy's already dieing go through
            if (enemyController.IsDieing())
            {
                return;
            }

            // Otherwise deal damage
            enemyController.Damage(projectileDmg);
        }

        // Push the object slightly based on impact force
        other.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
        // Play bullet hitting audio
        audioManager.Play("BulletHit");
        Destroy(gameObject);
    }
}
