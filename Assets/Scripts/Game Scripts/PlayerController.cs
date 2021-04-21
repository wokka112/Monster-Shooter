using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    private PlayerAnim playerAnim;

    public float speed = 1000.0f;

    public float verticalInput;
    public float horizontalInput;
    public float zBound = 9;
    public float xBound = 17;

    // How high off the ground the player is (y-axis)
    private float playerHeight;

    //TODO remove after done testing
    //For testing
    private bool pauseGame = false;

    private bool hasLightningShield = false;

    private Vector3 newVelocity;
    private Camera cam;
    private Rigidbody playerRb;
    public GameObject lightningShieldIndicator;
    private AudioManager audioManager;
    private HealthSystem playerHealth;
    public GameObject bulletSpawnPoint;

    public CrosshairAnimation crosshairAnimation;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        cam = Camera.main;
        playerHeight = transform.position.y;
        playerAnim = GetComponentInChildren<PlayerAnim>();
        playerHealth = GetComponent<HealthSystem>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void FixedUpdate()
    {
        if (!GameManager.gameOver)
        {
            // Remove current velocity from new velocity to cancel out accumulation of force/velocity
            playerRb.AddForce(newVelocity - playerRb.velocity, ForceMode.VelocityChange);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameOver && !PauseMenu.gameIsPaused)
        {
            GetInputs();
            SetMovementAnimation();
            CalculateNewVelocity();

            // pauseGame if statement for testing
            if (!pauseGame)
            {
                FollowCursor();
            }

            // Rotate to follow cursor
            // FollowCursor();

            // Click to shoot towards cursor
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ShootProjectile();
            }

            //for testing and pictures
            // Press space to pause the game
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pauseGame = !pauseGame;
                
                if (pauseGame)
                {
                    Time.timeScale = 0f;
                } else
                {
                    Time.timeScale = 1f;
                }
            }
        }

        // Keep lightning shield indicator positioned under player
        lightningShieldIndicator.transform.position = gameObject.transform.position + new Vector3(0, -playerHeight, 0);
    }

    private void GetInputs()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // If the player is trying to go above or below the upper or lower boundary, stop them
        if ((transform.position.z < -zBound && verticalInput < 0)
            || (transform.position.z > zBound && verticalInput > 0))
        {
            verticalInput = 0;
        }

        // If the player is trying to go beyond the left or right boundary, stop them
        if ((transform.position.x < -xBound && horizontalInput < 0)
            || (transform.position.x > xBound && horizontalInput > 0))
        {
            horizontalInput = 0;
        }
    }
    
    private void SetMovementAnimation()
    {
        // If the player is moving at all
        if (verticalInput > 0 || verticalInput < 0
            || horizontalInput > 0 || horizontalInput < 0)
        {
            // Set the animation to run
            playerAnim.Run();
        } // Otherwise 
        else
        {
            // Set the animation to idle
            playerAnim.Idle();
        }
    }

    private void CalculateNewVelocity()
    {
        // Calculate new velocity for character to move
        newVelocity = Vector3.zero;
        newVelocity += cam.transform.right * horizontalInput * Time.deltaTime * speed;
        newVelocity += cam.transform.up * verticalInput * Time.deltaTime * speed;
    }

    private void FollowCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(transform.up, transform.position);
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        plane.Raycast(ray, out distance);

        transform.LookAt(ray.GetPoint(distance));
    }

    private void ShootProjectile()
    {
        playerAnim.Shoot();
        Instantiate(projectilePrefab, bulletSpawnPoint.transform.position, transform.rotation);
    }

    public void HitByEnemyProjectile(int damage)
    {
        // If you have an active lightning shield
        if (hasLightningShield)
        {
            // Lose the lightning shield but live
            DeactivateLightningShield();
        } // Otherwise 
        else
        {
            // Take damage
            Damage(damage);
        }
    }
    public void Damage(int damage)
    {
        if (!GameManager.gameOver)
        {
            // Take damage
            playerHealth.Damage(damage);

            // If you're at 0 or less health
            if (playerHealth.getHealth() <= 0)
            {
                // Die and lose
                Lose();
            } // Otherwise
            else
            {
                // Play getting hit sound
            }
        }
    }

    private void DeactivateLightningShield()
    {
        // Turn off lightning shield
        hasLightningShield = false;
        // Hide indicator
        lightningShieldIndicator.SetActive(false);
        // Stop active lightning shield sound looping
        audioManager.Stop("LightningShieldActive");
    }


    private void Lose()
    {
        // Set the game to over
        FindObjectOfType<GameManager>().GameOver();
        // Kill player
        Die();
    }

    private void Die()
    {
        audioManager.Play("PlayerDeath");
        playerAnim.Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lightning Shield"))
        {
            audioManager.Play("LightningShieldPickup");
            Destroy(other.gameObject);
            // Turn on lightning shield
            hasLightningShield = true;
            // Display lightning shield indicator
            lightningShieldIndicator.SetActive(true);
            audioManager.Play("LightningShieldActive");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            bool enemyIsDieing = collision.gameObject.GetComponent<EnemyController>().IsDieing();
            if (hasLightningShield && !enemyIsDieing)
            {
                // Play lightning sound
                collision.gameObject.GetComponent<EnemyController>().Damage(999);
                DeactivateLightningShield();
                audioManager.Play("LightningShieldHit");
            }
        }
    }
}
