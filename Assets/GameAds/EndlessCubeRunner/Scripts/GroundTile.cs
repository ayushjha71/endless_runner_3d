using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;

    public GameObject obstaclesPrefab;
    public GameObject coinPrefab;
    public GameObject shieldPrefab;
    public GameObject speedBoostPrefab;
    public GameObject extraCoinsPrefab; // Cluster of coins

    [Header("Spawn Probabilities")]
    [Range(0, 1)] public float coinSpawnChance = 0.7f;
    [Range(0, 1)] public float shieldSpawnChance = 0.1f;
    [Range(0, 1)] public float speedBoostSpawnChance = 0.1f;
    [Range(0, 1)] public float extraCoinsSpawnChance = 0.15f;

    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        SpawnObstacles();
        SpawnCollectibles();
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile();
        Destroy(gameObject, 2);
    }

    void SpawnObstacles()
    {
        int obstacleSpawnIndex = Random.Range(3, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
        Instantiate(obstaclesPrefab, spawnPoint.position, Quaternion.identity, transform);
    }

    void SpawnCollectibles()
    {
        // Spawn regular coins
        if (Random.value <= coinSpawnChance)
        {
            int coinsSpawnIndex = Random.Range(6, 8);
            Transform coinSpawnPoint = transform.GetChild(coinsSpawnIndex).transform;
            Instantiate(coinPrefab, coinSpawnPoint.position, Quaternion.identity, transform);
        }

        // Spawn power-ups (only one per tile)
        float powerUpRoll = Random.value;
        if (powerUpRoll <= shieldSpawnChance)
        {
            SpawnAtRandomChild(shieldPrefab, 6, 8);
        }
        else if (powerUpRoll <= shieldSpawnChance + speedBoostSpawnChance)
        {
            SpawnAtRandomChild(speedBoostPrefab, 6, 8);
        }
        else if (powerUpRoll <= shieldSpawnChance + speedBoostSpawnChance + extraCoinsSpawnChance)
        {
            //SpawnAtRandomChild(extraCoinsPrefab, 6, 8);
        }
    }

    void SpawnAtRandomChild(GameObject prefab, int minChildIndex, int maxChildIndex)
    {
        int spawnIndex = Random.Range(minChildIndex, maxChildIndex);
        Transform spawnPoint = transform.GetChild(spawnIndex).transform;
        Instantiate(prefab, spawnPoint.position, Quaternion.identity, transform);
    }
}