using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private float score = 0f;


    void Update()
    {
        score += Time.deltaTime;
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }
}
