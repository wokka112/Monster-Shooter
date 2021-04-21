using UnityEngine;

public class MeleeAI : MonoBehaviour
{
    public float attackRate = 2.0f;

    private EnemyController enemyControllerScript;
    private float timeSinceLastAttack = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemyControllerScript = GetComponent<EnemyController>();
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
        enemyControllerScript.SetTouchingPlayer(true);

        if (collision.gameObject.CompareTag("Player"))
        {
            enemyControllerScript.MeleeAttack();
            timeSinceLastAttack = 0;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        enemyControllerScript.SetTouchingPlayer(false);
    }
}
