using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace gameAds.Manager
{
    public static class UIFadeEffect
    {
        public static void FadeIn(CanvasGroup canvasGroup, float endValue, float duration, Action OnCompleted)
        {
            canvasGroup.DOFade(endValue, duration).OnComplete(() => { OnCompleted?.Invoke(); });
        }

        public static void FadeOut(CanvasGroup canvasGroup, float endValue, float duration, Action OnCompleted)
        {
            canvasGroup.DOFade(endValue, duration).OnComplete(() => { OnCompleted?.Invoke(); });
        }
    }

    public static class CustomEvents
    {
        public static Action<PlayerMovement> OnGetPlayerMovementHandler;
        public static Action<int> OnCoinCOllected;

        public static Action<float> OnPowerUpStatus;
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
        }

        private IEnumerator WaitForAdsToLoad()
        {
            yield return new WaitForSeconds(waitTime);            
            AdsManager.Instance.BannerAdsHandler.ShowBannerAds();
        }

        private void OnDestroy()
        {
            AdsManager.Instance.BannerAdsHandler.HideBannerAds();
        }
    }
}