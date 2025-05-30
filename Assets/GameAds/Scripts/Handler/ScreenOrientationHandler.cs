using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenOrientationHandler : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) 
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }
}
