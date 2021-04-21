using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float destroyDelay = 1.0f;
    public float speed;
    public int scoreValue;
    public float rangedAttackDelay = 0.5f;
    public int strength = 1;

    public AudioSource meleeAttackAudio;
    public AudioSource rangedAttackAudio;
    public AudioSource getHitAudio;
    public AudioSource movementAudio;
    public AudioSource deathAudio;

    public GameObject enemyProjectilePrefab;

    private GameManager gameManager;
    private EnemyAnim enemyAnimScript;
    private Rigidbody enemyRb;
    private RigidbodyConstraints initialRbConstraints;
    private HealthSystem enemyHealth;
    private Transform playerTransform;
    private Vector3 newVelocity;
    private PlayerController playerController;

    private bool isDieing = false;
    private bool allowMovement = true;
    private bool touchingPlayer = false;

    public bool IsDieing()
    {
        return isDieing;
    }

    public bool IsTouchingPlayer() { return touchingPlayer; }
    public void SetTouchingPlayer(bool touchingPlayer) { this.touchingPlayer = touchingPlayer; }

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimScript = GetComponent<EnemyAnim>();
        enemyRb = GetComponent<Rigidbody>();
        initialRbConstraints = enemyRb.constraints;
        enemyHealth = GetComponent<HealthSystem>();
        gameManager = FindObjectOfType<GameManager>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
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

    public void DamagePlayer()
    {
        if (touchingPlayer)
        {
            playerController.Damage(strength);
        }
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
            Invoke("SpawnRangedProjectile", rangedAttackDelay);
        }
    }

    public void Damage(int damage)
    {
        // Deal damage to enemy health
        enemyHealth.Damage(damage);

        // If enemy's health is 0 or less
        if (enemyHealth.getHealth() <= 0)
        {
            // Kill enemy
            Die();
        } 
        // Otherwise
        else
        {
            // Play getting hit audio
            getHitAudio.Play();
        }        
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

    private void OnCollisionEnter(Collision collision)
    {
        // If the enemy collides with a player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Stop the player pushing the enemy
            enemyRb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // IF enemy is no longer colliding with player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reset constraints so enemy can move again
            enemyRb.constraints = initialRbConstraints;
        }
    }
}
