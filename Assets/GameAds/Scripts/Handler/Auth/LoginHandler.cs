using UnityEngine;
using PlayFab.ClientModels;
using gameAds.Constant;

namespace gameAds.Handler.Auth
{
    public class LoginHandler : MonoBehaviour
    {
        public RegisterPlayFabUserRequest RegisterRequest
        {
            get;
            private set;
        }

        public LoginWithEmailAddressRequest LoginRequest 
        { 
            get; 
            private set; 
        }

        public SendAccountRecoveryEmailRequest ForgetRequest
        {
            get;
            private set;
        }

        public string ProfileName;
        public string ProfileEmail;

        public static LoginHandler Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        public void LoginUser(string email,  string password)
        {
            LoginRequest = new LoginWithEmailAddressRequest
            {
                Email = email,
                Password = password,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };
        }

        public void RegisterUser(string username, string userEmail, string password)
        {
            RegisterRequest = new RegisterPlayFabUserRequest
            {
                Username = username,
                Email = userEmail,
                Password = password,
                RequireBothUsernameAndEmail = true
            };
            //ProfileName = username;
            //ProfileEmail = userEmail;
            PlayerPrefs.SetString(GameAdsConstant.UserName, username);
            PlayerPrefs.SetString(GameAdsConstant.UserEmail, userEmail);
            PlayerPrefs.Save();
        }

        public void ForgetEmail(string email)
        {
            ForgetRequest = new SendAccountRecoveryEmailRequest
            {
                Email = email,
                TitleId = "1CD465"
            };
        }
    }
}
