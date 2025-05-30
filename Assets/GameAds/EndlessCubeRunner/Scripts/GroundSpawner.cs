using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;
    public GameObject groundTile;
    public int initialTileCount = 15;
    private Vector3 nextSpawnPoint;
    private float tileLength;
    private int currentTileIndex = 0; // Track which tile we're spawning

    void Start()
    {
        // Get the length of the ground tile from its bounds
        tileLength = meshRenderer.bounds.size.z;
        // Spawn initial tiles
        for (int i = 0; i < initialTileCount; i++)
        {
            SpawnTile();
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

        nextSpawnPoint.z += tileLength; // Move forward by tile length
        currentTileIndex++; // Increment for next tile
    }
}