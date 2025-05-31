using TMPro;
using UnityEngine;
using EndlessCubeRunner.Handler;
using EndlessCubeRunner.Constant;

namespace EndlessCubeRunner.Manager
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text distanceText;
        [SerializeField]
        private TMP_Text coinText;

        private float totalDistance = 0f;
        private int collectedCoins = 0;
        private Vector3 lastPosition;
        private PlayerMovement playerMovement;

        private void OnEnable()
        {
            EndlessRunnerConstant.OnGetPlayerMovementHandler += GetPlayerMovement;
            EndlessRunnerConstant.OnCoinCOllected += AddCoin;
        }

        private void OnDisable()
        {
            EndlessRunnerConstant.OnGetPlayerMovementHandler -= GetPlayerMovement;
            EndlessRunnerConstant.OnCoinCOllected -= AddCoin;
        }

        private void GetPlayerMovement(PlayerMovement player)
        {
            playerMovement = player;
            lastPosition = playerMovement.transform.position;
        }

        private void AddCoin(int coinAmount)
        {
            collectedCoins += coinAmount;
            UpdateUI();
        }

        void Update()
        {
            // Calculate distance traveled since last frame
            if (playerMovement != null)
            {
                float distanceThisFrame = Vector3.Distance(playerMovement.transform.position, lastPosition);
                totalDistance += distanceThisFrame;
                lastPosition = playerMovement.transform.position;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Display distance in meters with 1 decimal place
            distanceText.text = totalDistance.ToString("F1") + "m";
            coinText.text = collectedCoins.ToString();

            // Update GameManager with current values
            GameManager.Instance.TotalDistance = totalDistance;
            GameManager.Instance.TotalCoin = collectedCoins;

            // Check for high score before saving
            CheckAndSaveHighScore();
        }

        private void CheckAndSaveHighScore()
        {
            // Get the current high score from PlayerPrefs (default to 0 if not found)
            float lastHighScore = PlayerPrefs.GetFloat("HighScore", 0f);
            int lastHighCoins = PlayerPrefs.GetInt("HighScoreCoins", 0);

            // Check if current distance is a new high score
            if (totalDistance > lastHighScore)
            {
                // New high score achieved! Save both distance and coins
                PlayerPrefs.SetFloat("HighScore", totalDistance);
                PlayerPrefs.SetInt("HighScoreCoins", collectedCoins);
                PlayerPrefs.Save();

                // If you want to show a "New High Score!" message to the player TODO
            }
        }
    }
}