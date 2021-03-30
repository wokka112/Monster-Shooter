using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float destroyDelay = 1.0f;
    public float speed;
    public int scoreValue;
    public float rangedAttackDelay = 0.5f;

    public AudioSource meleeAttackAudio;
    public AudioSource rangedAttackAudio;
    public AudioSource hitAudio;
    public AudioSource movementAudio;
    public AudioSource deathAudio;

    public GameObject enemyProjectilePrefab;

    private GameManager gameManager;
    private EnemyAnim enemyAnimScript;
    private Rigidbody enemyRb;
    private Transform playerTransform;
    private Vector3 newVelocity;

    //TODO Replace with allowTracking and then trigger from enemyHealth?
    private bool isDieing = false;
    private bool allowMovement = true;

    //TODO remove the anim calls from here and call animator from different classes?

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimScript = GetComponent<EnemyAnim>();
        enemyRb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        StartMovement();
    }

    void FixedUpdate()
    {
        if (allowMovement && !isDieing && !GameManager.gameOver)
        {
            enemyRb.AddForce(newVelocity - enemyRb.velocity, ForceMode.VelocityChange);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameOver)
        {
            if (!isDieing)
            {
                LookAtPlayer();
                newVelocity = transform.forward * Time.deltaTime * speed;
            }
        }
    }

    public void StartMovement()
    {
        enemyAnimScript.StartRunning();
        allowMovement = true;
    }

    public void StopMovement()
    {
        enemyAnimScript.StopRunning();
        allowMovement = false;
    }

    public void MeleeAttack()
    {
        if (!isDieing)
        {
            enemyAnimScript.MeleeAttack();
            meleeAttackAudio.Play();
        }
    }

    public void RangedAttack()
    {
        if (!isDieing && !GameManager.gameOver)
        {
            enemyAnimScript.RangedAttack();
            rangedAttackAudio.Play();
            //TODO move into ranged ai script
            // Spawn ranged projectile and shoot forward
            Invoke("SpawnRangedProjectile", rangedAttackDelay);
        }
    }

    public void GetHit()
    {
        hitAudio.Play();
    }

    public void Die()
    {
        isDieing = true;
        // Play death anim, then destroy after several seconds.
        enemyAnimScript.Die();
        deathAudio.Play();
        gameManager.IncreaseScore(scoreValue);
        Invoke("DestroyEnemy", destroyDelay);
    }

    public bool IsDieing()
    {
        return isDieing;
    }

    public void Victory()
    {
        enemyAnimScript.Victory();
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void LookAtPlayer()
    {
        gameObject.transform.LookAt(playerTransform);
    }

    void SpawnRangedProjectile()
    {
        Instantiate(enemyProjectilePrefab, transform.position, transform.rotation);
    }
}
