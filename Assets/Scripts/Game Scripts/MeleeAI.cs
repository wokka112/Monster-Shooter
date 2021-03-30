using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : MonoBehaviour
{
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
