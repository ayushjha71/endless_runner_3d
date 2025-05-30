using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text ScoreText;
    int myScore = 000;
    // Start is called before the first frame update
    void Start()
    {
        ScoreText = this.GetComponent<Text>();
    }
    public void AddScore(int score)
    {
        myScore = myScore + score;
    }
    private void Update()
    {
        ScoreText.text = myScore.ToString();
    }
}
