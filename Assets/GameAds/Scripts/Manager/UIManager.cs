using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EndlessCubeRunner.Handler;
using EndlessCubeRunner.Handler.Auth;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;


namespace EndlessCubeRunner.Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private WalletHandler walletHandler;
        [SerializeField]
        private string sceneName;

        [Header("Panel")]
        [SerializeField]
        private GameObject inGameUI;
        [SerializeField]
        private GameObject profilePanel;
        [SerializeField]
        private GameObject addBankPanel;
        [SerializeField]
        private GameObject withdrawalPanel;
        [SerializeField]
        private GameObject walletPanel;
        [SerializeField]
        private RewardHandler rewardHandler;

        [Header("In Game UI")]
        [SerializeField]
        private Button profileBtn;
        [SerializeField]
        private Button playBtn;
        [SerializeField]
        private Button rewardedBTN;

        [Header("Profile")]
        [SerializeField]
        private TMP_Text profileName;
        [SerializeField]
        private TMP_Text profileEmail;
        [SerializeField]
        private Button closeBtn;
        [SerializeField]
        private Button addBankBtn;
        [SerializeField]
        private Button withdrawalBtn;
        [SerializeField]
        private Button walletBtn;
        [SerializeField]
        private Button logoutButton;

        private bool canEnable;

        public GameObject ProfilePanel => profilePanel;
        public TMP_Text ProfileName => profileName;
        public TMP_Text ProfileEmail => profileEmail;

        private void Start()
        {
            playBtn.onClick.AddListener(OnClickPlayButton);
            rewardedBTN.onClick.AddListener(OnClickRewardedBtn);

            profileBtn.onClick.AddListener(OnClickProfileBtn);
            closeBtn.onClick.AddListener(OnClickCloseBtn);

            addBankBtn.onClick.AddListener(OnClickAddBankBtn);
            withdrawalBtn.onClick.AddListener(OnClickWithdrawal);
            walletBtn.onClick.AddListener(OnClickWalletButton);
            

            logoutButton.onClick.AddListener(OnClickLogoutBtn);

            if (!Advertisement.isInitialized)
            {
                rewardedBTN.gameObject.SetActive(false);
                canEnable = true;
            }
        }

        private void OnClickLogoutBtn()
        {
            ProfileName.text = "Name";
            ProfileEmail.text = "Email";
        }

        private void Update()
        {
            if (Advertisement.isInitialized)
            {
                if (canEnable)
                {
                    rewardedBTN.gameObject.SetActive(true);
                    canEnable = false;
                }
            }
        }

        private void OnClickPlayButton()
        {
            SceneManager.LoadScene(sceneName);
        }

        private void OnClickWithdrawal()
        {
            DisableAllParentPanel();
            withdrawalPanel.SetActive(true);

        }

        private void OnClickWalletButton()
        {
            DisableAllParentPanel();
            walletPanel.SetActive(true);
            walletHandler.TotalCoinText.text = walletHandler.currentCoins.ToString();
            walletHandler.TotalBalanceText.text = walletHandler.balance.ToString();
        }

        private void OnClickAddBankBtn()
        {
            DisableAllParentPanel();
            addBankPanel.SetActive(true);
        }

        public void OnClickCloseBtn()
        {
            DisableAllParentPanel();
            inGameUI.SetActive(true);
        }

        private void OnClickProfileBtn()
        {
            DisableAllParentPanel();
            profileName.text = PlayerPrefs.GetString("UserName");
            ProfileEmail.text = PlayerPrefs.GetString("UserEmail");
            Debug.Log("Profile Nmae");
            profilePanel.SetActive(true);

        }

        private void OnClickRewardedBtn()
        {
            AdsManager.Instance.RewardedAdsHandler.ShowRewardedAds();
        }

        public void OpenProfilePanel()
        {
            DisableAllParentPanel();
            ProfilePanel.SetActive(true);
        }

        public void DisableAllParentPanel()
        {
            inGameUI.SetActive(false);
            profilePanel.SetActive(false);
            addBankPanel.SetActive(false);
            withdrawalPanel.SetActive(false);
            walletPanel.SetActive(false);
        }

    }
}