using System.Collections.Generic;
using UnityEngine;

namespace EndlessCubeRunner.Handler
{
    public class GroundSpawnerHandler : MonoBehaviour
    {
        public GameObject groundTile;

        [Header("Infinite Generation Settings")]
        [SerializeField]
        private Transform playerTransform; // Assign your player here
        [SerializeField]
        private int tilesAhead = 5; // How many tiles to keep ahead of player
        [SerializeField]
        private int maxTilesInScene = 5; // Maximum tiles to keep in scene

        private Vector3 mNextSpawnPoint;
        private float mTileLength = 32; //Hard coded value of road length 
        private int mCurrentTileIndex = 0; // Track which tile we're spawning

        // For infinite generation
        private Queue<GameObject> mSpawnedTiles = new Queue<GameObject>();
        private bool IsInfiniteGeneration = false;

        void Start()
        {
            if (playerTransform != null)
            {
                IsInfiniteGeneration = true;
            }

            // Spawn initial tiles
            for (int i = 0; i < tilesAhead; i++)
            {
                SpawnTile();
            }
        }

        void Update()
        {
            // Only run infinite generation if enabled
            if (IsInfiniteGeneration && playerTransform != null)
            {
                // Check if we need to spawn more tiles ahead of player
                float playerZ = playerTransform.position.z;
                float spawnThreshold = playerZ + (tilesAhead * mTileLength);

                // Spawn new tiles if player is getting close to the end
                if (mNextSpawnPoint.z < spawnThreshold)
                {
                    SpawnTile();
                }

                // Remove old tiles that are far behind the player
                RemoveOldTiles(playerZ);
            }
        }

        public void SpawnTile()
        {
            Quaternion groundRotation = Quaternion.Euler(0, 0, 0);
            GameObject newTile = Instantiate(groundTile, mNextSpawnPoint, groundRotation);

            // Get the ObstacleSpawner component and set its index
            ObstacleSpawnerHandler obstacleSpawner = newTile.GetComponentInChildren<ObstacleSpawnerHandler>();
            if (obstacleSpawner != null)
            {
                obstacleSpawner.SetGroundTileIndex(mCurrentTileIndex);
            }

            // Add to spawned tiles queue for infinite generation
            if (IsInfiniteGeneration)
            {
                mSpawnedTiles.Enqueue(newTile);
            }

            mNextSpawnPoint.z += mTileLength; // Move forward by tile length
            mCurrentTileIndex++; // Increment for next tile
        }

        private void RemoveOldTiles(float playerZ)
        {
            // Remove tiles that are too far behind the player
            while (mSpawnedTiles.Count > maxTilesInScene)
            {
                GameObject oldTile = mSpawnedTiles.Dequeue();
                if (oldTile != null)
                {
                    // Only destroy if it's far enough behind the player
                    float tileZ = oldTile.transform.position.z;
                    if (tileZ < playerZ - (mTileLength * 1)) // Keep 3 tiles behind player
                    {
                        Destroy(oldTile);
                    }
                    else
                    {
                        // Put it back in queue if not far enough
                        mSpawnedTiles.Enqueue(oldTile);
                        break;
                    }
                }
            }
        }
    }
}