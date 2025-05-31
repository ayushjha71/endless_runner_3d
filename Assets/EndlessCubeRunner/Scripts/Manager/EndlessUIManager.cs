using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EndlessCubeRunner.Handler;
using EndlessCubeRunner.Constant;

namespace EndlessCubeRunner.Manager
{
    public class EndlessUIManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private GameObject bgAudio;
        [SerializeField]
        private GroundSpawnerHandler groundSpawner;
        [Header("Canvas")]
        [SerializeField]
        private CanvasGroup mainCanvas;
        [SerializeField]
        private CanvasGroup InGameCanvas;
        [SerializeField]
        private CanvasGroup gameOverCanvas;
        [Header("Menu Canvas")]
        [SerializeField]
        private Button startBtn;
        [SerializeField]
        private GameObject startPanel;
        [SerializeField]
        private Button settingBtn;
        [SerializeField]
        private GameObject settingPanel;
        [SerializeField]
        private Button leaderBoardBtn;
        [SerializeField]
        private GameObject leaderBoardPanel;
        [SerializeField]
        private TMP_Text leaderBoardCoinText;
        [SerializeField]
        private TMP_Text leaderBoardDistanceText;
        [SerializeField]
        private Button exitBtn;

        [Header("Game Over Canvas")]
        [SerializeField]
        private Button restartButton;
        [SerializeField]
        private Button rewardLivesButton;
        [SerializeField]
        private TMP_Text totalCoinEarn;
        [SerializeField]
        private TMP_Text distanceTravel;

        private void Start()
        {
            leaderBoardCoinText.text = PlayerPrefs.GetInt("HighScoreCoins").ToString();
            leaderBoardDistanceText.text = PlayerPrefs.GetFloat("HighScore").ToString();

            DisableAllCanvas();
            if (GameManager.Instance.UserFirstVisit)
            {
                mainCanvas.gameObject.SetActive(true);
                startPanel.SetActive(true);
                GameManager.Instance.UserFirstVisit = false;
            }
            else
            {
                EndlessRunnerConstant.FadeIn(InGameCanvas, 1, 0.3f, null);
                mainCanvas.gameObject.SetActive(false);
                groundSpawner.playerTransform.gameObject.SetActive(true);
                bgAudio.SetActive(true);

            }

            startBtn.onClick.AddListener(OnClickStartBtn);
            settingBtn.onClick.AddListener(OnClickSettingBtn);
            leaderBoardBtn.onClick.AddListener(OnClickLeaderBoardBtn);
            exitBtn.onClick.AddListener(OnClickExitBtn);

            restartButton.onClick.AddListener(RestartButtonClick);
            rewardLivesButton.onClick.AddListener(OnClickIncreaseLivesBtn);
        }

        private void OnEnable()
        {
            EndlessRunnerConstant.OnGameOver += GameOver;
        }

        private void OnDisable()
        {
            EndlessRunnerConstant.OnGameOver -= GameOver;
        }

        private void Update()
        {
            if (GameManager.Instance.CanGetReward)
            {
                gameOverCanvas.gameObject.SetActive(false);
                GameManager.Instance.CanGetReward = false;
            }
        }

        private void GameOver()
        {
            GameManager.Instance.PlayAudio(GameManager.Instance.GameOver, audioSource);
            gameOverCanvas.gameObject.SetActive(true);
            EndlessRunnerConstant.FadeIn(gameOverCanvas, 1, 0.3f, () =>
            {
                bgAudio.SetActive(false);
                Time.timeScale = 0f;
                totalCoinEarn.text = GameManager.Instance.TotalCoin.ToString();
                distanceTravel.text = GameManager.Instance.TotalDistance.ToString("F1") + "m";
            });
        }

        private void RestartButtonClick()
        {
            GameManager.Instance.PlayAudio(GameManager.Instance.ClickAudio, audioSource);
            AdsManager.Instance.InterstitialAdsHandler.ShowIntersitialAds();
        }

        private void OnClickIncreaseLivesBtn()
        {
            GameManager.Instance.PlayAudio(GameManager.Instance.ClickAudio, audioSource);
            AdsManager.Instance.RewardedAdsHandler.ShowRewardedAds();
        }

        private void OnClickStartBtn()
        {
            GameManager.Instance.PlayAudio(GameManager.Instance.ClickAudio, audioSource);
            DisableAllCanvas();
            startPanel.SetActive(true);
            EndlessRunnerConstant.FadeOut(mainCanvas, 0, 0.3f, () =>
            {
                EndlessRunnerConstant.FadeIn(InGameCanvas, 1, 0.3f, null);
                groundSpawner.playerTransform.gameObject.SetActive(true);
                mainCanvas.gameObject.SetActive(false);
                bgAudio.SetActive(true);
            });
        }

        private void OnClickSettingBtn()
        {
            GameManager.Instance.PlayAudio(GameManager.Instance.ClickAudio, audioSource);
            DisableAllCanvas();
            settingPanel.SetActive(true);
        }

        private void OnClickLeaderBoardBtn()
        {
            GameManager.Instance.PlayAudio(GameManager.Instance.ClickAudio, audioSource);
            DisableAllCanvas();
            leaderBoardPanel.SetActive(true);
        }

        private void OnClickExitBtn()
        {
            GameManager.Instance.PlayAudio(GameManager.Instance.ClickAudio, audioSource);
            Application.Quit();
        }

        private void DisableAllCanvas()
        {
            settingPanel.SetActive(false);
            leaderBoardPanel.SetActive(false);
            startPanel.SetActive(false);
        }
    }
}
