using UnityEngine;

namespace EndlessRunner.Controller
{
    public enum ObstacleState
    {
        Cloud,
        Obstacle
    }

    public class ObstacleMoverController : MonoBehaviour
    {
        [Header("Speed Settings")]
        public float baseSpeed = 5f;
        [SerializeField] private float maxSpeed = 15f;
        [SerializeField] private float speedIncreaseRate = 2f;

        [Header("Boundary Settings")]
        [SerializeField] private float leftBoundary = -15f;
        [SerializeField] private float rightBoundary = 15f;

        [Header("State")]
        [SerializeField] private ObstacleState state = ObstacleState.Obstacle;

        private float currentSpeed;

        private void Start()
        {
            currentSpeed = baseSpeed;
        }

        private void Update()
        {
            if (state == ObstacleState.Obstacle)
            {
                currentSpeed = Mathf.Min(currentSpeed + speedIncreaseRate * Time.deltaTime, maxSpeed);
            }

            switch (state)
            {
                case ObstacleState.Obstacle:
                    transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
                    if (transform.position.x < leftBoundary)
                    {
                        Destroy(gameObject);
                    }
                    break;

                case ObstacleState.Cloud:
                    transform.Translate(Vector3.right * baseSpeed * Time.deltaTime);
                    if (transform.position.x > rightBoundary)
                    {
                        Destroy(gameObject);
                    }
                    break;
            }
        }
        public float GetCurrentSpeed()
        {
            return currentSpeed;
        }

        public void ResetSpeed()
        {
            currentSpeed = baseSpeed;
        }
    }
} 