using EndlessCubeRunner.Constant;
using UnityEngine;
using UnityEngine.Advertisements;

namespace EndlessCubeRunner.Handler.Ads
{
    public class InitializeAdsHandler : MonoBehaviour, IUnityAdsInitializationListener
    {
        private string mGameKey;


        private void Awake()
        {
#if UNITY_ANDROID
            mGameKey = GameAdsConstant.ANDROID_GAME_KEY;
#elif UNITY_IOS
            mGameKey = GameAdsConstant.IOS_GAME_KEY;
#elif UNITY_EDITOR
            mGameKey = GameAdsConstant.ANDROID_GAME_KEY;  
#endif
            if(!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(mGameKey, GameAdsConstant.IsTesting, this);
            }
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Adds Initialized");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log("Adds Initialized Failed!");
        }

    }
}
