using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    private PlayerAnim playerAnim;

    public float speed = 1000.0f;

    public float verticalInput;
    public float horizontalInput;
    public float zBound = 9;
    public float xBound = 23;

    // How high off the ground the player is (y-axis)
    private float playerHeight;

    //TODO remove after done testing
    //For testing
    private bool pausePlayer = false;

    private bool hasLightningShield = false;

    private Vector3 newVelocity;
    private Camera cam;
    private Rigidbody playerRb;
    public GameObject lightningShieldIndicator;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        cam = Camera.main;
        playerHeight = transform.position.y;
        playerAnim = GetComponentInChildren<PlayerAnim>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void FixedUpdate()
    {
        // Move character using physics
        // Remove current velocity from new velocity to cancel out accumulation of force/velocity
        if (!GameManager.gameOver)
        {
            playerRb.AddForce(newVelocity - playerRb.velocity, ForceMode.VelocityChange);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameOver && GameManager.gameIsActive && !PauseMenu.gameIsPaused)
        {
            SetInputs();
            SetMovementAnimation();
            CalculateNewVelocity();

            // Rotation follows cursor
            // pausePlayer clause for testing
            if (!pausePlayer)
            {
                FollowCursor();
            }

            // Click fire at cursor
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ShootProjectile();
            }

            //for testing
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pausePlayer = !pausePlayer;
            }

            // If player collides with enemy in attack mode, die
        }

        lightningShieldIndicator.transform.position = gameObject.transform.position + new Vector3(0, -playerHeight, 0);
    }

    public void HitByEnemyProjectile()
    {
        if (hasLightningShield)
        {
            DeactivateLightningShield();
        } else
        {
            Lose();
        }
    }

    private void SetInputs()
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
        if (verticalInput > 0 || verticalInput < 0
            || horizontalInput > 0 || horizontalInput < 0)
        {
            playerAnim.Run();
        } else
        {
            playerAnim.Idle();
        }
    }

    private void CalculateNewVelocity()
    {
        // Set new velocity for character to move
        newVelocity = Vector3.zero;
        newVelocity += cam.transform.right * horizontalInput * Time.deltaTime * speed;
        newVelocity += cam.transform.up * verticalInput * Time.deltaTime * speed;
    }

    private void FollowCursor()
    {
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        // Setting playerHeight keeps the player looking straight ahead.
        mousePosition = new Vector3(mousePosition.x, playerHeight, mousePosition.z);
        transform.LookAt(mousePosition);
    }

    private void ShootProjectile()
    {
        playerAnim.Shoot();
        Instantiate(projectilePrefab, transform.position, transform.rotation);
    }

    private void DeactivateLightningShield()
    {
        hasLightningShield = false;
        lightningShieldIndicator.SetActive(false);
        audioManager.Stop("LightningShieldActive");
    }

    private void Lose()
    {
        FindObjectOfType<GameManager>().GameOver();
        Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lightning Shield"))
        {
            // Play pickup sound
            audioManager.Play("LightningShieldPickup");
            Destroy(other.gameObject);
            hasLightningShield = true;
            lightningShieldIndicator.SetActive(true);
            audioManager.Play("LightningShieldActive");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hasLightningShield)
            {
                // Play lightning sound
                collision.gameObject.GetComponent<HealthSystem>().Damage(999);
                DeactivateLightningShield();
                audioManager.Play("LightningShieldHit");
            } else
            {
                Lose();
            }
        }
    }

    private void Die()
    {
        audioManager.Play("PlayerDeath");
        //Play death anim
        playerAnim.Die();
    }
}
