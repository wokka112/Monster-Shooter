using System.Collections;
using System.Collections.Generic;
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
            // If enemy's already dieing go through
            if (other.GetComponent<EnemyController>().IsDieing())
            {
                return;
            }

            // Otherwise deal damage
            other.GetComponent<HealthSystem>().Damage(projectileDmg);
        }

        other.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
        audioManager.Play("BulletHit");
        Destroy(gameObject);
    }
}
