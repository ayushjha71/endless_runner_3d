using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

namespace gameAds.Handler.Auth
{
    public class AuthUIHandler : MonoBehaviour
    {
        [SerializeField]
        private LoginHandler loginHandler;

        [Header("Auth Panels")]
        [SerializeField]
        private GameObject loginPanel;
        [SerializeField]
        private GameObject signUpPanel;
        [SerializeField]
        private GameObject forgetPanel;

        [Header("Login")]
        [SerializeField]
        private TMP_InputField loginEmailInput;
        [SerializeField]
        private TMP_InputField loginPasswordInput;
        [SerializeField]
        private Button loginBtn;
        [SerializeField]
        private Button createAccountBtn;
        [SerializeField]
        private Button forgetPasswordBtn;
        [SerializeField]
        private Toggle rememberMe;
        [SerializeField]
        private TMP_Text loginErrorText;

        [Header("Sign Up")]
        [SerializeField]
        private TMP_InputField nameInput;
        [SerializeField]
        private TMP_InputField emailInput;
        [SerializeField]
        private TMP_InputField passwordInput;
        [SerializeField]
        private TMP_InputField confirmInput;
        [SerializeField]
        private Button signUpBtn;
        [SerializeField]
        private Button backBtn;
        [SerializeField]
        private TMP_Text signUpErrorText;

        [Header("Forget")]
        [SerializeField]
        private TMP_InputField forgetEmailInput;
        [SerializeField]
        private TMP_Text forgetStatusText;
        [SerializeField]
        private Button submitBtn;
        [SerializeField]
        private Button forgetBackBtn;

        private void Start()
        {
            // Clear all error messages on start
            ClearAllErrorMessages();

            // Add button listeners
            loginBtn.onClick.AddListener(LoginBtn);
            signUpBtn.onClick.AddListener(SignUp);
            createAccountBtn.onClick.AddListener(OpenSignUpPanel);
            backBtn.onClick.AddListener(OpenLoginPanel);

            forgetPasswordBtn.onClick.AddListener(OpenForgetPanel);
            submitBtn.onClick.AddListener(SubmitBtn);
            forgetBackBtn.onClick.AddListener(ForgetBackBtn);
        }

        private void ClearAllErrorMessages()
        {
            loginErrorText.text = "";
            signUpErrorText.text = "";
            forgetStatusText.text = "";
        }

        private void LoginBtn()
        {
            // Clear previous error
            loginErrorText.text = "";

            // Field validation
            if (string.IsNullOrEmpty(loginEmailInput.text) || string.IsNullOrEmpty(loginPasswordInput.text))
            {
                loginErrorText.text = "Please enter both email and password";
                return;
            }

            // Show loading state
            loginErrorText.text = "Logging in...";
            loginBtn.interactable = false;

            LoginHandler.Instance.LoginUser(loginEmailInput.text, loginPasswordInput.text);
            PlayFabClientAPI.LoginWithEmailAddress(
                LoginHandler.Instance.LoginRequest,
                OnLoginSuccess,
                error => {
                    // Handle login error
                    loginBtn.interactable = true;
                    HandleLoginError(error);
                });
        }

        private void OnLoginSuccess(LoginResult result)
        {
            loginErrorText.text = "Login successful!";

            // Store user data if needed
            if (result.InfoResultPayload != null && result.InfoResultPayload.PlayerProfile != null)
            {
                Debug.Log("Player ID: " + result.InfoResultPayload.PlayerProfile.PlayerId);
            }

            // Load next scene
            SceneManager.LoadScene("MenuScene");
        }

        private void HandleLoginError(PlayFabError error)
        {
            string errorMessage;

            switch (error.Error)
            {
                case PlayFabErrorCode.AccountNotFound:
                    errorMessage = "Account not found. Please check your email or create a new account.";
                    break;
                case PlayFabErrorCode.InvalidEmailOrPassword:
                    errorMessage = "Invalid email or password. Please try again.";
                    break;
                case PlayFabErrorCode.InvalidParams:
                    errorMessage = "Invalid email format. Please check your email address.";
                    break;
                default:
                    errorMessage = "Login failed: " + error.GenerateErrorReport();
                    break;
            }

            loginErrorText.text = errorMessage;
        }

        private void SignUp()
        {
            // Clear previous error
            signUpErrorText.text = "";

            // Field validation
            if (string.IsNullOrEmpty(nameInput.text) ||
                string.IsNullOrEmpty(emailInput.text) ||
                string.IsNullOrEmpty(passwordInput.text) ||
                string.IsNullOrEmpty(confirmInput.text))
            {
                signUpErrorText.text = "Please fill in all fields";
                return;
            }

            if (passwordInput.text != confirmInput.text)
            {
                signUpErrorText.text = "Passwords don't match";
                return;
            }

            if (passwordInput.text.Length < 6)
            {
                signUpErrorText.text = "Password must be at least 6 characters";
                return;
            }

            // Show loading state
            signUpErrorText.text = "Creating account...";
            signUpBtn.interactable = false;

            LoginHandler.Instance.RegisterUser(nameInput.text, emailInput.text, passwordInput.text);
            PlayFabClientAPI.RegisterPlayFabUser(
                LoginHandler.Instance.RegisterRequest,
                OnRegisterSuccess,
                error => {
                    // Handle registration error
                    signUpBtn.interactable = true;
                    HandleRegisterError(error);
                });
        }

        private void OnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            signUpErrorText.text = "Account created successfully!";

            // Clear fields and switch to login panel after delay
            Invoke("SwitchToLoginAfterRegister", 1.5f);
        }

        private void SwitchToLoginAfterRegister()
        {
            // Clear fields
            nameInput.text = "";
            emailInput.text = "";
            passwordInput.text = "";
            confirmInput.text = "";

            // Switch to login panel
            DisablePanel();
            loginPanel.SetActive(true);
            signUpBtn.interactable = true;
        }

        private void HandleRegisterError(PlayFabError error)
        {
            string errorMessage;

            switch (error.Error)
            {
                case PlayFabErrorCode.EmailAddressNotAvailable:
                    errorMessage = "Email already in use. Try logging in or use a different email.";
                    break;
                case PlayFabErrorCode.UsernameNotAvailable:
                    errorMessage = "Username already taken. Please choose another one.";
                    break;
                default:
                    errorMessage = "Registration failed: " + error.GenerateErrorReport();
                    break;
            }

            signUpErrorText.text = errorMessage;
        }

        private void ForgetBackBtn()
        {
            forgetEmailInput.text = "";
            forgetStatusText.text = "";
            DisablePanel();
            loginPanel.SetActive(true);
        }

        private void SubmitBtn()
        {
            // Clear previous status
            forgetStatusText.text = "";

            // Field validation
            if (string.IsNullOrEmpty(forgetEmailInput.text))
            {
                forgetStatusText.text = "Please enter your email address";
                return;
            }

            if (!forgetEmailInput.text.Contains("@") || !forgetEmailInput.text.Contains("."))
            {
                forgetStatusText.text = "Please enter a valid email address";
                return;
            }

            // Show loading state
            forgetStatusText.text = "Sending recovery email...";
            submitBtn.interactable = false;

            LoginHandler.Instance.ForgetEmail(forgetEmailInput.text);
            PlayFabClientAPI.SendAccountRecoveryEmail(
                LoginHandler.Instance.ForgetRequest,
                OnForgetSuccess,
                error => {
                    // Handle forget password error
                    submitBtn.interactable = true;
                    HandleForgetError(error);
                });
        }

        private void OnForgetSuccess(SendAccountRecoveryEmailResult result)
        {
            forgetStatusText.text = "Recovery email sent! Please check your inbox.";
            submitBtn.interactable = true;

            // Clear field and switch to login panel after delay
            Invoke("SwitchToLoginAfterForget", 2f);
        }

        private void SwitchToLoginAfterForget()
        {
            forgetEmailInput.text = "";
            DisablePanel();
            loginPanel.SetActive(true);
        }

        private void HandleForgetError(PlayFabError error)
        {
            string errorMessage;

            switch (error.Error)
            {
                case PlayFabErrorCode.AccountNotFound:
                    errorMessage = "No account found with this email address.";
                    break;
                case PlayFabErrorCode.InvalidParams:
                    errorMessage = "Invalid email address format.";
                    break;
                default:
                    errorMessage = "Failed to send recovery email: " + error.GenerateErrorReport();
                    break;
            }

            forgetStatusText.text = errorMessage;
        }

        private void OpenLoginPanel()
        {
            // Clear all fields and errors when switching panels
            loginEmailInput.text = "";
            loginPasswordInput.text = "";
            loginErrorText.text = "";

            DisablePanel();
            loginPanel.SetActive(true);
        }

        private void OpenSignUpPanel()
        {
            // Clear all fields and errors when switching panels
            nameInput.text = "";
            emailInput.text = "";
            passwordInput.text = "";
            confirmInput.text = "";
            signUpErrorText.text = "";

            DisablePanel();
            signUpPanel.SetActive(true);
        }

        private void OpenForgetPanel()
        {
            // Clear field and error when opening panel
            forgetEmailInput.text = "";
            forgetStatusText.text = "";

            DisablePanel();
            forgetPanel.SetActive(true);
        }

        private void DisablePanel()
        {
            loginPanel.SetActive(false);
            signUpPanel.SetActive(false);
            forgetPanel.SetActive(false);
        }
    }
}