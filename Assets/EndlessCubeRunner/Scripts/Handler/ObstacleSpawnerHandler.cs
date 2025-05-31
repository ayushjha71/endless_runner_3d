using System.Collections.Generic;
using UnityEngine;

namespace EndlessCubeRunner.Handler
{
    public class ObstacleSpawnerHandler : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField]
        private GameObject[] obstaclePrefabs; // Array of obstacle prefabs
        [SerializeField]
        private GameObject coinPrefab;
        [SerializeField]
        private GameObject[] powerUpPrefabs; // Shield, speed boost, etc.

        [Header("Base Spawn Settings")]
        [SerializeField]
        private float minSpaceBetweenObjects = 3f;
        [SerializeField]
        private int baseMaxObstacles = 2;
        [SerializeField]
        private int maxObstaclesIncrementPerTile = 1; // How many more obstacles per tile

        [Header("Collectible Chances")]
        [Range(0f, 1f)]
        [SerializeField]
        private float coinSpawnChance = 0.4f;
        [Range(0f, 1f)]
        [SerializeField]
        private float powerUpSpawnChance = 0.3f;



        private float mStartZ = 6f; // Start spawning from this Z position
        private float mEndZ = 35f;   // End spawning at this Z position
        private float mLeftBoundary = -1.3f; //Hard Coded value
        private float mRightBoundary = 1.8f; // Hard Coded value
        private List<Vector3> mSpawnedPositions = new List<Vector3>();
        private int mGroundTileIndex = 0; // Will be set by GroundSpawner


        void Start()
        {
            SpawnContent();
        }

        public void SetGroundTileIndex(int index)
        {
            mGroundTileIndex = index;
        }

        void SpawnContent()
        {
            mSpawnedPositions.Clear();

            // Spawn obstacles first
            SpawnObstacles();

            // Then spawn collectibles in remaining spaces
            SpawnCollectibles();
            SpawnPowerUps();
        }

        void SpawnObstacles()
        {
            if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;

            // Calculate max obstacles based on tile index
            int currentMaxObstacles = baseMaxObstacles + (mGroundTileIndex * maxObstaclesIncrementPerTile);
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
                    mSpawnedPositions.Add(spawnPos);
                }
            }
        }

        void SpawnCollectibles()
        {
            // Spawn coins - increase chance based on tile index
            float currentCoinChance = Mathf.Min(coinSpawnChance + (mGroundTileIndex * 0.05f), 0.8f);
            if (coinPrefab != null && Random.value < currentCoinChance)
            {
                SpawnCoins();
            }
        }

        private void SpawnPowerUps()
        {
            // Spawn power-ups - slightly increase chance based on tile index
            float currentPowerUpChance = Mathf.Min(powerUpSpawnChance + (mGroundTileIndex * 0.02f), 0.5f);
            if (powerUpPrefabs != null && powerUpPrefabs.Length > 0 && Random.value < currentPowerUpChance)
            {
                SpawnPowerUp();
            }
        }

        void SpawnCoins()
        {
            // Increase coin count based on tile index
            int baseCoinCount = Random.Range(1, 4); // 1-3 coins
            int additionalCoins = Mathf.Min(mGroundTileIndex / 2, 3); // Add up to 3 more coins
            int coinCount = baseCoinCount + additionalCoins;

            for (int i = 0; i < coinCount; i++)
            {
                Vector3 spawnPos = GetRandomSpawnPosition();

                if (spawnPos != Vector3.zero)
                {
                    Instantiate(coinPrefab, spawnPos, Quaternion.identity, transform);
                    mSpawnedPositions.Add(spawnPos);
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
                mSpawnedPositions.Add(spawnPos);
            }
        }

        Vector3 GetRandomSpawnPosition()
        {
            int maxAttempts = 20;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                // Generate random position within road boundaries
                Vector3 randomPos = new Vector3(Random.Range(mLeftBoundary, mRightBoundary), 0.3f, Random.Range(mStartZ, mEndZ));

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
            foreach (Vector3 usedPos in mSpawnedPositions)
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
            Vector3 center = transform.position + new Vector3(0, 0, (mStartZ + mEndZ) / 2f);
            Vector3 size = new Vector3(mRightBoundary - mLeftBoundary, 0.1f, mEndZ - mStartZ);
            Gizmos.DrawWireCube(center, size);
        }
    }
}