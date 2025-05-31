using DG.Tweening;
using gameAds.Manager;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndlessUIManager : MonoBehaviour
{
    [SerializeField]
    private GroundSpawner groundSpawner;
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
            UIFadeEffect.FadeIn(InGameCanvas, 1, 0.3f, null);
            mainCanvas.gameObject.SetActive(false);
            groundSpawner.playerTransform.gameObject.SetActive(true);

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
        CustomEvents.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        CustomEvents.OnGameOver -= GameOver;
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
        gameOverCanvas.gameObject.SetActive(true);
        UIFadeEffect.FadeIn(gameOverCanvas,1, 0.3f, () =>
        {
            Time.timeScale = 0f;
            totalCoinEarn.text = GameManager.Instance.TotalCoin.ToString();
            distanceTravel.text = GameManager.Instance.TotalDistance.ToString("F1") + "m";
        });   
    }

    private void RestartButtonClick()
    {
        AdsManager.Instance.InterstitialAdsHandler.ShowIntersitialAds();
    }

    private void OnClickIncreaseLivesBtn()
    {
        AdsManager.Instance.RewardedAdsHandler.ShowRewardedAds();
    }

    private void OnClickStartBtn()
    {
        DisableAllCanvas();
        startPanel.SetActive(true);
        UIFadeEffect.FadeOut(mainCanvas, 0, 0.3f, () =>
        {
            UIFadeEffect.FadeIn(InGameCanvas, 1, 0.3f, null);
            groundSpawner.playerTransform.gameObject.SetActive(true);
            mainCanvas.gameObject.SetActive(false);
        });
    }

    private void OnClickSettingBtn()
    {
        DisableAllCanvas();
        settingPanel.SetActive(true);
    }

    private void OnClickLeaderBoardBtn()
    {
        DisableAllCanvas();
        leaderBoardPanel.SetActive(true);
    }

    private void OnClickExitBtn()
    {
        Application.Quit();
    }

    private void DisableAllCanvas()
    {
        settingPanel.SetActive(false);
        leaderBoardPanel.SetActive(false);
        startPanel.SetActive(false);
    }
}
