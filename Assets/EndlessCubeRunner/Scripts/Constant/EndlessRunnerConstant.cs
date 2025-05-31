using EndlessCubeRunner.Handler;
using DG.Tweening;
using UnityEngine;
using System;

namespace EndlessCubeRunner.Constant
{
    public static class EndlessRunnerConstant
    {
        public const string ANDROID_GAME_KEY = "5786102";
        public const string IOS_GAME_KEY = "5786103";
        public static bool IsTesting = false;

        public static void FadeIn(CanvasGroup canvasGroup, float endValue, float duration, Action OnCompleted)
        {
            canvasGroup.DOFade(endValue, duration).OnComplete(() => { OnCompleted?.Invoke(); });
        }

        public static void FadeOut(CanvasGroup canvasGroup, float endValue, float duration, Action OnCompleted)
        {
            canvasGroup.DOFade(endValue, duration).OnComplete(() => { OnCompleted?.Invoke(); });
        }

        public static Action<PlayerMovement> OnGetPlayerMovementHandler;
        public static Action<int> OnCoinCOllected;

        public static Action<float> OnPowerUpStatus;
        public static Action OnGameOver;
    }
}
