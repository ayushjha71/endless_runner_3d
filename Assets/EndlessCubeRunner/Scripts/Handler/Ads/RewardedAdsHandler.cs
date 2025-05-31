using UnityEngine;
using EndlessCubeRunner.Manager;
using UnityEngine.Advertisements;

namespace EndlessCubeRunner.Handler.Ads
{
    public class RewardedAdsHandler : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField]
        private string androidAdUnitId;
        [SerializeField]
        private string iosAdUnitId;


        private string mAdUnitID;

        private void Awake()
        {
#if UNITY_ANDROID
            mAdUnitID = androidAdUnitId;
#elif UNITY_IOS
            mAdUnitID = iosAdUnitId;
#endif
        }

        public void LoadRewardedAds()
        {
            Advertisement.Load(mAdUnitID, this);
        }

        public void ShowRewardedAds()
        {
            Advertisement.Show(mAdUnitID, this);
            LoadRewardedAds();
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("Ads Loading Completed Intersitial");
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log("Ads Loading Failed Intersitial");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {

        }

        public void OnUnityAdsShowStart(string placementId)
        {

        }

        public void OnUnityAdsShowClick(string placementId)
        {

        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Debug.LogError("Give User a reward he has finished ads watching");
            GameManager.Instance.CanGetReward = true;
            Time.timeScale = 1;
        }
    }
}