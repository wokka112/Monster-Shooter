using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAI : MonoBehaviour
{
    public float attackRange = 8.0f;
    public float attackRate = 2.0f;

    private EnemyController enemyControllerScript;
    private Transform playerTransform;
    private float timeSinceLastAttack = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemyControllerScript = GetComponent<EnemyController>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is within attack range, attack them
        if (Vector3.Distance(transform.position, playerTransform.position) < attackRange)
        {
            // Stop moving
            enemyControllerScript.StopMovement();

            // If the last attack was > attackRate seconds ago, attack
            if (timeSinceLastAttack > attackRate)
            {
                enemyControllerScript.RangedAttack();
                timeSinceLastAttack = 0;
            }
        }
        // If the player is outside attack range, move towards them
        else
        {
            enemyControllerScript.StartMovement();
        }

        timeSinceLastAttack += Time.deltaTime;
    }

    void StartMovement()
    {
        enemyControllerScript.StartMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyControllerScript.StopMovement();
            enemyControllerScript.MeleeAttack();
            timeSinceLastAttack = 0;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        float movementDelay = timeSinceLastAttack + attackRate;

        if (movementDelay < 0)
        {
            movementDelay = 0;
        }

        // Start moving once it's been attackRate seconds since the last attack
        Invoke("StartMovement", movementDelay);
    }
}
