using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text distanceText;
    [SerializeField]
    private TMP_Text coinText;
    [SerializeField]
    private PlayerMovement playerMovement; // Reference to your PlayerMovement script

    public float totalDistance = 0f;
    private float collectedCoins = 0f;
    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = playerMovement.transform.position;
    }

    public void AddCoin(int coinAmount)
    {
        collectedCoins += coinAmount;
        UpdateUI();
    }

    void Update()
    {
        // Calculate distance traveled since last frame
        float distanceThisFrame = Vector3.Distance(playerMovement.transform.position, lastPosition);
        totalDistance += distanceThisFrame;
        lastPosition = playerMovement.transform.position;

        UpdateUI();
    }

    private void UpdateUI()
    {
        // Display distance in meters with 1 decimal place
        distanceText.text = totalDistance.ToString("F1") + "m";
        coinText.text = collectedCoins.ToString();
    }

    // Public getters for other systems that might need these values
    public float GetTotalDistance() => totalDistance;
    public float GetCollectedCoins() => collectedCoins;
}