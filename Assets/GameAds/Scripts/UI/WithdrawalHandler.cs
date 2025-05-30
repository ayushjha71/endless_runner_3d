using TMPro;
using UnityEngine;
using UnityEngine.UI;
using gameAds.Constant;

public class WithdrawalHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_Text balanceText;
    [SerializeField]
    private TMP_Text accountNoDisplayText;
    [SerializeField]
    private Button withdrawalBtn;

    private int balance;

    private void Start()
    {
        withdrawalBtn.onClick.AddListener(OnClickWithdrawlBtn);
        accountNoDisplayText.text = PlayerPrefs.GetString(GameAdsConstant.ACCOUNT_NUMBER);
    }

    private void OnClickWithdrawlBtn()
    {
        balanceText.text = PlayerPrefs.GetInt(GameAdsConstant.Balance).ToString();
        if (PlayerPrefs.GetString(GameAdsConstant.ACCOUNT_NUMBER) != null)
        {
            accountNoDisplayText.text = "Account Number not found";
            Debug.Log("Account Number not found");
        }
        if(balance == 0 || balance <= 500)
        {
            Debug.Log("Insufficient Balance");
        }
    }
}
