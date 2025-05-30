using UnityEngine;

namespace EndlessRunner.Manager
{
    public class ObstacleSpawnerManager : MonoBehaviour
    {
        [SerializeField]
        private Transform floorSpawnPoint;
        [SerializeField]
        private Transform airSpawnPoint;
        public GameObject floorObstaclePrefab;
        public GameObject airObstacle1Prefab;
        public GameObject airObstacle2Prefab;
        [SerializeField]
        private GameObject[] cloud;
        [SerializeField]
        private float CloudTransformMinX;
        [SerializeField]
        private float CloudTransformMaxX;
        [SerializeField]
        private float CloudTransformMinY;
        [SerializeField]
        private float CloudTransformMaxY;
        public float minSpawnDelay = 1f;
        public float maxSpawnDelay = 3f;
        public float cloudSpawnInterval = 10; 

        private float nextCloudSpawnTime;

        void Start()
        {
            nextCloudSpawnTime = Time.time + cloudSpawnInterval;
            ScheduleNextObstacleSpawn();
        }

        void Update()
        {
            if (Time.time >= nextCloudSpawnTime)
            {
                SpawnCloud();
                nextCloudSpawnTime = Time.time + cloudSpawnInterval;
            }
        }

        void ScheduleNextObstacleSpawn()
        {
            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            Invoke(nameof(SpawnObstacle), randomDelay);
        }

        void SpawnObstacle()
        {
            int obstacleType = Random.Range(0, 3);
            GameObject obstacle = null;
            Vector3 spawnPosition;

            switch (obstacleType)
            {
                case 0:
                    obstacle = floorObstaclePrefab;
                    spawnPosition = floorSpawnPoint.position;
                    break;
                case 1:
                    obstacle = airObstacle1Prefab;
                    spawnPosition = floorSpawnPoint.position;
                    break;
                case 2:
                    obstacle = airObstacle2Prefab;
                    spawnPosition = airSpawnPoint.position;
                    break;
                default:
                    spawnPosition = Vector3.zero;
                    break;
            }

            if (obstacle != null)
            {
                Instantiate(obstacle, spawnPosition, Quaternion.identity);
            }

            ScheduleNextObstacleSpawn();
        }

        private void SpawnCloud()
        {
            int CloudType = Random.Range(0, cloud.Length);
            if (CloudType < cloud.Length && cloud[CloudType] != null)
            {
                Vector3 spawnPosition = new Vector3(
                    Random.Range(CloudTransformMinX, CloudTransformMaxX),
                    Random.Range(CloudTransformMinY, CloudTransformMaxY),
                    0
                );
                Instantiate(cloud[CloudType], spawnPosition, Quaternion.identity);
            }
        }
    }
}