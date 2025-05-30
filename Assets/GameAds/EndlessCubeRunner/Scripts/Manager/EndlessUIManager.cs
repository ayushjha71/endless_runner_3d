using DG.Tweening;
using gameAds.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private Button settingBtn;
    [SerializeField]    
    private Button leaderBoardBtn;
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
        DisableAllCanvas();

        if (GameManager.Instance.UserFirstVisit)
        {
            mainCanvas.gameObject.SetActive(true);
            GameManager.Instance.UserFirstVisit = false;
        }
        else
        {
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
        Time.timeScale = 0f;
        gameOverCanvas.gameObject.SetActive(true);
        totalCoinEarn.text = GameManager.Instance.TotalCoin.ToString();
        distanceTravel.text = GameManager.Instance.TotalDistance.ToString("F1") + "m";
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
        mainCanvas.gameObject.SetActive(false);
        InGameCanvas.gameObject.Serialize(true);
        groundSpawner.playerTransform.gameObject.SetActive(true);
    }

    private void OnClickSettingBtn()
    {
        mainCanvas.gameObject.SetActive(false);
    }

    private void OnClickLeaderBoardBtn()
    {
        CustomEvents.OnLeaderBoard?.Invoke();
        mainCanvas.gameObject.SetActive(false);
    }

    private void OnClickExitBtn()
    {
        Application.Quit();
    }

    private void DisableAllCanvas()
    {
        mainCanvas.gameObject.SetActive(false);
        InGameCanvas.gameObject.Serialize(true);
        gameOverCanvas.gameObject.SetActive(false);
    }
}
