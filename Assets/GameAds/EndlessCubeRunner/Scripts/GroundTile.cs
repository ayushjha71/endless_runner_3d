using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;

    public GameObject obstaclesPrefab;
    public GameObject coinPrefab;

    // Start is called before the first frame update
    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        SpawnObstacles();
        SpawnCoins();
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

    void SpawnCoins()
    {
        int coinsSpawnIndex = Random.Range(6, 8);
        Transform coinSpawnPoint = transform.GetChild(coinsSpawnIndex).transform;
        Instantiate(coinPrefab, coinSpawnPoint.position, Quaternion.identity, transform);
    }
}
