using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileLogic : MonoBehaviour
{
    public float impactForce = 5f;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit " + other.gameObject.name);
            other.GetComponentInParent<PlayerController>().HitByEnemyProjectile();
        }

        // Play particle explosion
        audioManager.Play("EnemyProjectileHit");
        Destroy(gameObject);
    }
}
