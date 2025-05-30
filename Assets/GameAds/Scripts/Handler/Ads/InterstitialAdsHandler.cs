using gameAds.Manager;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

namespace gameAds.Handler.Ads
{
    public class InterstitialAdsHandler : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
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

        public void LoadIntersitialAds()
        {
            Advertisement.Load(mAdUnitID, this);
        }

        public void ShowIntersitialAds()
        {
            Advertisement.Show(mAdUnitID, this);
            LoadIntersitialAds();
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
            Debug.Log("Give User a reward he has finished ads watching");
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
