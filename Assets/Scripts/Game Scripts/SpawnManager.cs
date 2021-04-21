using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject lightningShieldPrefab;
    public GameObject gameManager;

    // Pickup spawning is bounded within an inner square, away from the edges so player can't be killed
    // by something offscreen as they pick up the pickup.
    public float pickupSpawnBoundZ = 7f;
    public float pickupSpawnBoundX = 15f;

    public float enemySpawnBoundZ = 10f;
    public float enemySpawnBoundX = 24f;

    // Makes sure enemies and pickups spawn above the ground rather than in it.
    private float ySpawnPos = 0.5f;

    // The rate at which pickups spawn with enemy waves.
    // A rate of 10 means there's a 1/10 chance of a pickup spawning with each enemy spawn.
    // A rate of 6 means there's a 1/6 chance.
    public int pickupSpawnRate = 6;

    // The rate at which enemy waves increase in size in seconds.
    // e.g. 10 means increase the number of enemies to spawn each wave by 1 every 10 seconds
    public float baseEnemyNoIncreaseRate = 10f;
    private float difficultyAdjustedEnemyNoIncreaseRate;

    // Bools to determine whether to spawn certain types of enemies.
    private bool spawnFastEnemies = false;
    private bool spawnTankEnemies = false;
    private bool spawnRangedEnemies = false;

    // The times after which specific enemy types start spawning.
    private float fastEnemySpawnStart = 20f;
    private float rangedEnemySpawnStart = 40f;
    private float tankEnemySpawnStart = 50f;

    // The indices at which certain enemies are stored in the prefabs.
    // TODO change this system
    private const int FAST_ENEMY_INDEX = 1;
    private const int RANGED_ENEMY_INDEX = 2;
    private const int TANK_ENEMY_INDEX = 3;
    
    // Game difficulty must be greater than 0
    public void StartSpawning(int gameDifficulty)
    {
        // Sets spawn rate based on selected game difficulty
        difficultyAdjustedEnemyNoIncreaseRate = baseEnemyNoIncreaseRate;
        difficultyAdjustedEnemyNoIncreaseRate /= gameDifficulty;

        // Start off giving 1 pickup after a second
        Invoke("SpawnPickup", 1.0f);
        // Start spawning enemies
        StartCoroutine(SpawnEnemies());
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnEnemies()
    {
        // Spawn enemies while the game is active
        while (!GameManager.gameOver)
        {
            // If the time passed in the game is greater than any of the enemy type's spawning start times
            // sets the relevant spawn bool to true.
            if (GameManager.timeSurvived > fastEnemySpawnStart)
            {
                spawnFastEnemies = true;
            }

            if (GameManager.timeSurvived > rangedEnemySpawnStart)
            {
                spawnTankEnemies = true;
            }

            if (GameManager.timeSurvived > tankEnemySpawnStart)
            {
                spawnRangedEnemies = true;
            }

            // Wait a random number of seconds between spawning enemy waves
            float spawnDelay = Random.Range(3.0f, 5.0f);
            yield return new WaitForSeconds(spawnDelay);

            // Potentially spawn a pickup with the enemy wave.
            // There is a 1/pickupSpawnRate chance of a pickup spawning each time an enemy wave spawns.
            bool spawnPickup = Random.Range(0, pickupSpawnRate) == 0;

            if (spawnPickup)
            {
                SpawnPickup();
            }

            // Spawn a number of enemies in random positions based on how long the player has survived
            // and the difficulty setting of the game.
            for (int i = 0; i < GetNumberOfEnemiesToSpawn(); i++)
            {
                SpawnEnemy(ChooseEnemyType());
            }
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, GetRandomEnemySpawnPosition(), enemyPrefab.transform.rotation);
    }

    private Vector3 GetRandomEnemySpawnPosition()
    {
        // Start x and z positions at x and z spawn boundaries
        float xPos = enemySpawnBoundX;
        float zPos = enemySpawnBoundZ;

        // Determine whether the enemy should spawn along the x-axis or z-axis
        bool spawnOnXAxis = Random.Range(0, 2) == 0;

        // If the enemy spawns along the x-axis
        if (spawnOnXAxis)
        {
            // Select a random position for them to spawn on the x-axis
            xPos = Random.Range(-enemySpawnBoundX, enemySpawnBoundX);

            // Determine whether the enemy should spawn along the bottom of the play area
            bool spawnBottom = Random.Range(0, 2) == 0;
            // If the enemy should spawn at the bottom
            if (spawnBottom)
            {
                // Make zPos negative to spawn at bottom z-axis spawn boundary
                zPos *= -1;
            }
        }
        // else if enemy spawns along z-axis
        else
        {
            // Select a random position for them to spawn on the z-axis
            zPos = Random.Range(-enemySpawnBoundZ, enemySpawnBoundZ);

            // Determine whether the enemy should spawn along the right of the play area
            bool spawnRight = Random.Range(0, 2) == 0;
            // If the enemy should spawn on the right
            if (spawnRight)
            {
                // Make zPos negative to spawn at bottom z-axis spawn boundary
                xPos *= -1;
            }
        }

        return new Vector3(xPos, ySpawnPos, zPos);
    }

    private int GetNumberOfEnemiesToSpawn()
    {
        int noOfEnemies = (int)(GameManager.timeSurvived / difficultyAdjustedEnemyNoIncreaseRate);

        return noOfEnemies;
    }

    //TODO change
    private GameObject ChooseEnemyType()
    {
        int index = 0;

        //Tightly coupled, should think on how to decouple this a bit.
        if (spawnTankEnemies)
        {
            index = Random.Range(0, TANK_ENEMY_INDEX + 1);    
        } else if (spawnRangedEnemies)
        {
            index = Random.Range(0, RANGED_ENEMY_INDEX + 1);
        } else if (spawnFastEnemies)
        {
            index = Random.Range(0, FAST_ENEMY_INDEX + 1);
        }

        return enemyPrefabs[index];
    }

    private void SpawnPickup()
    {
        Instantiate(lightningShieldPrefab, GetRandomPickupSpawnPosition(), GetRandomPickupSpawnRotation());
    }

    private Vector3 GetRandomPickupSpawnPosition()
    {
        // Pickup should spawn within an inner box in the play area
        float xPos = Random.Range(-pickupSpawnBoundX, pickupSpawnBoundX);
        float zPos = Random.Range(-pickupSpawnBoundZ, pickupSpawnBoundZ);

        return new Vector3(xPos, ySpawnPos, zPos);
    }

    private Quaternion GetRandomPickupSpawnRotation()
    {
        float yRotation = Random.Range(0f, 360f);
        Quaternion rotation = lightningShieldPrefab.transform.rotation;
        rotation.y = yRotation;
        return rotation;
    }
}
