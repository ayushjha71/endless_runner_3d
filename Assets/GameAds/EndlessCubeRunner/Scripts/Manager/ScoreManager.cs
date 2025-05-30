using UnityEngine;
using TMPro;
using gameAds.Manager;

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
        CustomEvents.OnGetPlayerMovementHandler += GetPlayerMovement;
        CustomEvents.OnCoinCOllected += AddCoin;
    }

    private void OnDisable()
    {
        CustomEvents.OnGetPlayerMovementHandler -= GetPlayerMovement;
        CustomEvents.OnCoinCOllected -= AddCoin;
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
        if(playerMovement != null)
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
        GameManager.Instance.TotalDistance = totalDistance;
        GameManager.Instance.TotalCoin = collectedCoins;
    }

    // Public getters for other systems that might need these values
    public float GetTotalDistance() => totalDistance;
    public float GetCollectedCoins() => collectedCoins;
}