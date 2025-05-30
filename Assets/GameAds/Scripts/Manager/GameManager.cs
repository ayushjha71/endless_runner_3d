using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace gameAds.Manager
{
    public static class CustomEvents
    {
        public static Action<PlayerMovement> OnGetPlayerMovementHandler;
        public static Action<int> OnCoinCOllected;

        public static Action OnLeaderBoard;

        public static Action OnGameOver;
    }
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private float waitTime = 5;

        public bool CanGetReward = false;
        public bool UserFirstVisit = true;

        public static GameManager Instance;

        public int TotalCoin
        {
            get;
            set;
        }

        public float TotalDistance
        {
            get;
            set;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(WaitForAdsToLoad());
            LocalDataBackup.LoadData();
        }

        private IEnumerator WaitForAdsToLoad()
        {
            yield return new WaitForSeconds(waitTime);            
            AdsManager.Instance.BannerAdsHandler.ShowBannerAds();
        }

        private void OnDestroy()
        {
            AdsManager.Instance.BannerAdsHandler.HideBannerAds();
            LocalDataBackup.SaveData();
        }
    }
}