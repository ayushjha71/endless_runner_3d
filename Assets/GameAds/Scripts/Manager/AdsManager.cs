using gameAds.Handler;
using gameAds.Handler.Ads;
using UnityEngine;

namespace gameAds.Manager
{
    public class AdsManager : MonoBehaviour
    {
        [SerializeField]
        private InitializeAdsHandler initializeAdsHandler;
        [SerializeField]
        private BannerAdsHandler bannerAdsHandler;
        [SerializeField]
        private InterstitialAdsHandler interstitialAdsHandler;
        [SerializeField]
        private RewardedAdsHandler rewardedAdsHandler;


        public BannerAdsHandler BannerAdsHandler => bannerAdsHandler;
        public InterstitialAdsHandler InterstitialAdsHandler => interstitialAdsHandler;
        public RewardedAdsHandler RewardedAdsHandler => rewardedAdsHandler;


        public static AdsManager Instance
        {
            get;
            private set;
        }

        private void Awake()
        {
            if(Instance != null && Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            interstitialAdsHandler.LoadIntersitialAds();
            bannerAdsHandler.LoadBannerAds();
            rewardedAdsHandler.LoadRewardedAds();
        }
    }
}
