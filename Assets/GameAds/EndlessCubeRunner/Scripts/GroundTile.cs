using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs; // Array of obstacle prefabs
    public GameObject coinPrefab;
    public GameObject[] powerUpPrefabs; // Shield, speed boost, etc.

    [Header("Base Spawn Settings")]
    public int baseMaxObstacles = 3;
    public int maxObstaclesIncrementPerTile = 1; // How many more obstacles per tile

    [Header("Road Boundaries")]
    public float leftBoundary = -1.3f;
    public float rightBoundary = 1.8f;
    public float roadLength = 35f; // Length of this ground tile

    [Header("Spawn Zones")]
    public float startZ = 5f; // Start spawning from this Z position
    public float endZ = 35f;   // End spawning at this Z position

    [Header("Collectible Chances")]
    [Range(0f, 1f)] public float coinSpawnChance = 0.4f;
    [Range(0f, 1f)] public float powerUpSpawnChance = 0.3f;

    [Header("Spacing")]
    public float minSpaceBetweenObjects = 1.5f;

    private List<Vector3> spawnedPositions = new List<Vector3>();
    private int groundTileIndex = 0; // Will be set by GroundSpawner

    public void SetGroundTileIndex(int index)
    {
        groundTileIndex = index;
    }

    void Start()
    {
        SpawnContent();
    }

    void SpawnContent()
    {
        spawnedPositions.Clear();

        // Spawn obstacles first
        SpawnObstacles();

        // Then spawn collectibles in remaining spaces
        SpawnCollectibles();
    }

    void SpawnObstacles()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;

        // Calculate max obstacles based on tile index
        int currentMaxObstacles = baseMaxObstacles + (groundTileIndex * maxObstaclesIncrementPerTile);
        int obstacleCount = Random.Range(2, currentMaxObstacles + 1);

        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPosition();

            if (spawnPos != Vector3.zero)
            {
                // Choose random obstacle
                GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

                // Spawn obstacle
                GameObject spawnedObstacle = Instantiate(obstacleToSpawn, spawnPos, GetRandomRotation(), transform);

                // Record position
                spawnedPositions.Add(spawnPos);
            }
        }
    }

    void SpawnCollectibles()
    {
        // Spawn coins - increase chance based on tile index
        float currentCoinChance = Mathf.Min(coinSpawnChance + (groundTileIndex * 0.05f), 0.8f);
        if (coinPrefab != null && Random.value < currentCoinChance)
        {
            SpawnCoins();
        }

        // Spawn power-ups - slightly increase chance based on tile index
        float currentPowerUpChance = Mathf.Min(powerUpSpawnChance + (groundTileIndex * 0.02f), 0.5f);
        if (powerUpPrefabs != null && powerUpPrefabs.Length > 0 && Random.value < currentPowerUpChance)
        {
            SpawnPowerUp();
        }
    }

    void SpawnCoins()
    {
        // Increase coin count based on tile index
        int baseCoinCount = Random.Range(1, 4); // 1-3 coins
        int additionalCoins = Mathf.Min(groundTileIndex / 2, 3); // Add up to 3 more coins
        int coinCount = baseCoinCount + additionalCoins;

        for (int i = 0; i < coinCount; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPosition();

            if (spawnPos != Vector3.zero)
            {
                Instantiate(coinPrefab, spawnPos, Quaternion.identity, transform);
                spawnedPositions.Add(spawnPos);
            }
        }
    }

    void SpawnPowerUp()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();

        if (spawnPos != Vector3.zero)
        {
            GameObject powerUpToSpawn = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
            Instantiate(powerUpToSpawn, spawnPos, Quaternion.identity, transform);
            spawnedPositions.Add(spawnPos);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        int maxAttempts = 20;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            // Generate random position within road boundaries
            Vector3 randomPos = new Vector3(
                Random.Range(leftBoundary, rightBoundary),
                0f,
                Random.Range(startZ, endZ)
            );

            // Convert to world position
            Vector3 worldPos = transform.TransformPoint(randomPos);

            // Check if position is valid (not too close to other objects)
            if (IsPositionValid(worldPos))
            {
                return worldPos;
            }
        }

        return Vector3.zero; // Failed to find valid position
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 usedPos in spawnedPositions)
        {
            if (Vector3.Distance(position, usedPos) < minSpaceBetweenObjects)
            {
                return false;
            }
        }
        return true;
    }

    Quaternion GetRandomRotation()
    {
        // Random Y rotation for obstacles
        return Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + new Vector3(0, 0, (startZ + endZ) / 2f);
        Vector3 size = new Vector3(rightBoundary - leftBoundary, 0.1f, endZ - startZ);
        Gizmos.DrawWireCube(center, size);
    }
}