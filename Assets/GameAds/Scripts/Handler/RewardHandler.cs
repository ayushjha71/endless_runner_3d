using EndlessCubeRunner.Constant;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessCubeRunner.Handler
{
    public class RewardHandler : MonoBehaviour
    {
        //[SerializeField] private int rewardAmount = 10;
        [SerializeField]
        private TMP_Text coinText;
        [SerializeField]
        private GameObject rewardPopUp;
        [SerializeField]
        private GameObject coinCollectAnim;
        [SerializeField]
        private Button collectBtn;

        public TMP_Text CoinText => coinText;
        public GameObject RewardPopUp => rewardPopUp;
       
        private int totalCoins;


        private void Start()
        {
            LoadCoins();
            collectBtn.onClick.AddListener(OnClickCollect);
        }

        private void OnClickCollect()
        {
            StartCoroutine(CollectCoin());
            RewardPopUp.SetActive(false);
        }

        public IEnumerator CollectCoin()
        {
            coinCollectAnim.SetActive(true);
            yield return new WaitForSeconds(2);
            RewardCoins(10);
            coinCollectAnim.SetActive(false);
        }

        private void RewardCoins(int rewardAmount)
        {
            totalCoins += rewardAmount;
            SaveCoins(totalCoins);
            UpdateUI();
            Debug.LogError("Coins rewarded: " + rewardAmount + ", Total Coins: " + totalCoins);
        }

        private void SaveCoins(int totalCoins)
        {
            PlayerPrefs.SetInt(GameAdsConstant.TotalCoins, totalCoins);
            PlayerPrefs.Save();
        }

        private void LoadCoins()
        {
            totalCoins = PlayerPrefs.GetInt(GameAdsConstant.TotalCoins);
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (CoinText != null)
            {
                CoinText.text = totalCoins.ToString();
            }
        }
    }
}
