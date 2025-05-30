using UnityEngine;
using UnityEngine.Advertisements;

namespace gameAds.Handler.Ads
{
    public class BannerAdsHandler : MonoBehaviour
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

            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        }

        public void LoadBannerAds()
        {
            BannerLoadOptions loadOptions = new BannerLoadOptions
            {
                loadCallback = BannerLoaded,
                errorCallback = BannerLoadedError
            };

            Advertisement.Banner.Load(mAdUnitID,loadOptions);
        }

        public void ShowBannerAds()
        {
            BannerOptions options = new BannerOptions
            {
                showCallback = BannerShown,
                clickCallback = BannerClick,
                hideCallback = BannerHide
            };
            Advertisement.Banner.Show(mAdUnitID,options);
        }

        public void HideBannerAds()
        {
            Advertisement.Banner.Hide();
        }


        private void BannerHide()
        {
            
        }

        private void BannerClick()
        {
            
        }

        private void BannerShown()
        {
           
        }

        private void BannerLoadedError(string message)
        {
           // Debug.Log("Banner Ads Loaded Falided");
        }

        private void BannerLoaded()
        {
           // Debug.Log("Banner Ads Loaded");
        }
    }
}
