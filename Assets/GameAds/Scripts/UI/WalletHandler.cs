using gameAds.Constant;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletHandler : MonoBehaviour
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private TMP_Text totalCoinText;
    [SerializeField] private TMP_Text totalBalanceText;
    [SerializeField] private Button convetCointToCashBtn;
    [SerializeField] private GameObject coinConversionInstructionPanel;
    [SerializeField] private GameObject convertionPanel;
    [SerializeField] private TMP_InputField coinToCashInputField;
    [SerializeField] private Button convertBtn;
    [SerializeField] private TMP_Text conversionErrorText;
    //[SerializeField] private TMP_Text conversionSuccessText;

    public int balance;
    public int currentCoins;

    // Conversion rates
    private readonly Dictionary<int, int> conversionRates = new Dictionary<int, int>()
    {
        {500, 25},   // 500 coins = 25 INR
        {1000, 50},  // 1000 coins = 50 INR
        {2000, 100}  // 2000 coins = 100 INR (fixed typo from your example)
    };


    public TMP_Text TotalCoinText => totalCoinText;
    public TMP_Text TotalBalanceText => totalBalanceText;

    private void Start()
    {
        DisablePanel();
        currentCoins = PlayerPrefs.GetInt(GameAdsConstant.TotalCoins);
        totalCoinText.text = currentCoins.ToString();

        closeBtn.onClick.AddListener(DisablePanel);
        convetCointToCashBtn.onClick.AddListener(OnClickConvertCoinBtn);
        convertBtn.onClick.AddListener(CoinToCashCalculation);

        // Initialize balance (you might want to load this from PlayerPrefs)
        balance = PlayerPrefs.GetInt("PlayerBalance", 0);
        totalBalanceText.text = balance.ToString();
    }

    private void OnClickConvertCoinBtn()
    {
        currentCoins = PlayerPrefs.GetInt(GameAdsConstant.TotalCoins);

        if (currentCoins < 500) // Minimum conversion is 500 coins
        {
            DisablePanel();
            coinConversionInstructionPanel.SetActive(true);
        }
        else
        {
            DisablePanel();
            convertionPanel.SetActive(true);
            coinToCashInputField.text = ""; // Clear previous input
            conversionErrorText.text = "";
        }
    }

    private void CoinToCashCalculation()
    {
        conversionErrorText.text = "";
        // Validate input
        if (string.IsNullOrEmpty(coinToCashInputField.text))
        {
            return;
        }

        if (!int.TryParse(coinToCashInputField.text, out int coinsToConvert))
        {
            conversionErrorText.text = "Please enter a valid number";
            return;
        }

        currentCoins = PlayerPrefs.GetInt(GameAdsConstant.TotalCoins);

        // Check if player has enough coins
        if (coinsToConvert > currentCoins)
        {
            conversionErrorText.text = $"You don't have enough coins. You have {currentCoins} coins.";
            return;
        }

        // Check if the amount matches one of the conversion rates
        if (!conversionRates.ContainsKey(coinsToConvert))
        {
            conversionErrorText.text = "Invalid conversion amount. Valid amounts are: 500, 1000, or 2000 coins.";
            return;
        }

        // Perform conversion
        int cashAmount = conversionRates[coinsToConvert];

        // Update player's coins and balance
        int newCoinAmount = currentCoins - coinsToConvert;
        int newBalance = balance + cashAmount;

       
        // Update UI
        totalCoinText.text = newCoinAmount.ToString();
        totalBalanceText.text = newBalance.ToString();
        currentCoins = newCoinAmount;
        balance = newBalance;

        PlayerPrefs.SetInt(GameAdsConstant.TotalCoins, newCoinAmount);
        PlayerPrefs.SetInt(GameAdsConstant.Balance, balance);
        PlayerPrefs.Save();
        // Show success message
        conversionErrorText.text = $"Successfully converted {coinsToConvert} coins to {cashAmount} INR!";

        // Clear input field
        coinToCashInputField.text = "";

        DisablePanel();
    }

    private void DisablePanel()
    {
        convertionPanel.SetActive(false);
        coinConversionInstructionPanel.SetActive(false);
        conversionErrorText.text = "";
    }
}