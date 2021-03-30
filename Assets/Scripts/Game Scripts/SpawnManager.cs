using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject lightningShieldPrefab;

    public float pickupSpawnBoundZ = 7f;
    public float pickupSpawnBoundX = 20f;

    public float enemySpawnBoundZ = 10f;
    public float enemySpawnBoundX = 24f;

    // The rate at which pickups spawn with enemy waves.
    // A rate of 10 means there's a 1/10 chance of a pickup spawning with each enemy spawn
    public int pickupSpawnRate = 6;

    // The rate at which enemy waves increase in size in seconds.
    // e.g. 5 means increase the number of enemies to spawn each wave by 1 every 10 seconds
    public float enemyNoIncreaseRate = 10f;

    public GameObject gameManager;
    private GameManager gameManagerScript;

    private bool spawnFastEnemies = false;
    private bool spawnTankEnemies = false;
    private bool spawnRangedEnemies = false;

    private int FAST_ENEMY_INDEX = 1;
    private int TANK_ENEMY_INDEX = 2;
    private int RANGED_ENEMY_INDEX = 3;

    private float fastEnemySpawnStart = 20f;
    private float rangedEnemySpawnStart = 40f;
    private float tankEnemySpawnStart = 75f;

    public void StartSpawning(int gameDifficulty)
    {
        enemyNoIncreaseRate = 10f;
        enemyNoIncreaseRate /= gameDifficulty;

        // Start off giving 1 pickup after a second
        Debug.Log("Started spawning");
        Invoke("SpawnPickup", 1.0f);
        Debug.Log("Invoked pickup");
        // Start spawning enemies
        StartCoroutine(SpawnEnemies());
        Debug.Log("Started coroutine");
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnEnemies()
    {
        while (GameManager.gameIsActive)
        {
            Debug.Log("In spawn enemies coroutine");
            if (GameManager.time > fastEnemySpawnStart)
            {
                spawnFastEnemies = true;
            }

            if (GameManager.time > rangedEnemySpawnStart)
            {
                spawnTankEnemies = true;
            }

            if (GameManager.time > tankEnemySpawnStart)
            {
                spawnRangedEnemies = true;
            }

            float spawnDelay = Random.Range(3.0f, 5.0f);
            yield return new WaitForSeconds(spawnDelay);

            // Pickups are spawned based on chance. There is a 1/pickupSpawnRate chance of a pickup spawning each time an enemy wave spawns.
            bool spawnPickup = Random.Range(0, pickupSpawnRate) == 0;

            if (spawnPickup)
            {
                SpawnPickup();
            }

            for (int i = 0; i < GetNumberOfEnemies(); i++)
            {
                Debug.Log("Spawning enemies");
                SpawnEnemy(ChooseEnemyType());
            }
        }
    }

    /*
    private void SpawnEnemies()
    {
        if (!GameManager.gameOver)
        {
            // 1 in 10 chance of spawning a pickup per enemy wave
            bool spawnPickup = Random.Range(0, pickupSpawnRate) == 0;

            if (spawnPickup)
            {
                SpawnPickup();
            }


            for (int i = 0; i < GetNumberOfEnemies(); i++)
            {
                SpawnEnemy(ChooseEnemyType());
            }

            float invokeDelay = Random.Range(3.0f, 5.0f);
            Invoke("SpawnEnemies", invokeDelay);
        }
    }
    */

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, GetRandomEnemySpawnPosition(), enemyPrefab.transform.rotation);
    }

    private Vector3 GetRandomEnemySpawnPosition()
    {
        bool top = (Random.Range(0, 2) == 0);
        bool left = (Random.Range(0, 2) == 0);
        bool spawnAnywhereOnX = (Random.Range(0, 2) == 0);

        float zPos = enemySpawnBoundZ;
        float xPos = enemySpawnBoundX;
        float yPos = 0.5f;
        
        if(!top)
        {
            zPos *= -1;
        }

        if (!left)
        {
            xPos *= -1;
        }

        if (spawnAnywhereOnX)
        {
            xPos = Random.Range(-enemySpawnBoundX, enemySpawnBoundX);   
        } else
        {
            zPos = Random.Range(-enemySpawnBoundZ, enemySpawnBoundZ);
        }

        return new Vector3(xPos, yPos, zPos);
    }

    private int GetNumberOfEnemies()
    {
        int noOfEnemies = (int) GameManager.time / 5;

        return noOfEnemies;
    }

    private GameObject ChooseEnemyType()
    {
        int index = 0;

        //Tightly coupled, should think on how to decouple this a bit.
        if (spawnRangedEnemies)
        {
            index = Random.Range(0, RANGED_ENEMY_INDEX + 1);    
        } else if (spawnTankEnemies)
        {
            index = Random.Range(0, TANK_ENEMY_INDEX + 1);
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
        float yPos = 0.5f;
        float xPos = Random.Range(-pickupSpawnBoundX, pickupSpawnBoundX);
        float zPos = Random.Range(-pickupSpawnBoundZ, pickupSpawnBoundZ);

        return new Vector3(xPos, yPos, zPos);
    }

    private Quaternion GetRandomPickupSpawnRotation()
    {
        float yRotation = Random.Range(0f, 360f);
        Quaternion rotation = lightningShieldPrefab.transform.rotation;
        rotation.y = yRotation;
        return rotation;
    }
}
