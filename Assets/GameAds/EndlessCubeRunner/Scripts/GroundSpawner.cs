using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile;

    [Header("Infinite Generation Settings")]
    public Transform playerTransform; // Assign your player here
    public int tilesAhead = 5; // How many tiles to keep ahead of player
    public int maxTilesInScene = 5; // Maximum tiles to keep in scene

    private Vector3 nextSpawnPoint;
    private float tileLength = 32;
    private int currentTileIndex = 0; // Track which tile we're spawning

    // For infinite generation
    private Queue<GameObject> spawnedTiles = new Queue<GameObject>();
    private bool infiniteGeneration = false;

    void Start()
    {
        if (playerTransform != null)
        {
            infiniteGeneration = true;
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
        if (infiniteGeneration && playerTransform != null)
        {
            // Check if we need to spawn more tiles ahead of player
            float playerZ = playerTransform.position.z;
            float spawnThreshold = playerZ + (tilesAhead * tileLength);

            // Spawn new tiles if player is getting close to the end
            if (nextSpawnPoint.z < spawnThreshold)
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
        GameObject newTile = Instantiate(groundTile, nextSpawnPoint, groundRotation);

        // Get the ObstacleSpawner component and set its index
        ObstacleSpawner obstacleSpawner = newTile.GetComponentInChildren<ObstacleSpawner>();
        if (obstacleSpawner != null)
        {
            obstacleSpawner.SetGroundTileIndex(currentTileIndex);
        }

        // Add to spawned tiles queue for infinite generation
        if (infiniteGeneration)
        {
            spawnedTiles.Enqueue(newTile);
        }

        nextSpawnPoint.z += tileLength; // Move forward by tile length
        currentTileIndex++; // Increment for next tile
    }

    private void RemoveOldTiles(float playerZ)
    {
        // Remove tiles that are too far behind the player
        while (spawnedTiles.Count > maxTilesInScene)
        {
            Debug.Log("Destroy");
            GameObject oldTile = spawnedTiles.Dequeue();
            if (oldTile != null)
            {
                // Only destroy if it's far enough behind the player
                float tileZ = oldTile.transform.position.z;
                if (tileZ < playerZ - (tileLength * 1)) // Keep 3 tiles behind player
                {
                    Destroy(oldTile);
                    Debug.Log("Destroy");
                }
                else
                {
                    // Put it back in queue if not far enough
                    spawnedTiles.Enqueue(oldTile);
                    break;
                }
            }
        }
    }
}