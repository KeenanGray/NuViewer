using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Score : MonoBehaviour
{
    public int TargetScore;
    public UnityEvent onTargetScoreReached;

    int currentScore;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        currentScore = 0;
    }
    void Update()
    {
        if (currentScore >= TargetScore)
            onTargetScoreReached.Invoke();

        if(scoreText!=null)
            scoreText.text=currentScore.ToString();
    }

    public void IncreaseScore(int amt)
    {
        currentScore += amt;
    }
    public void DecreaseScore(int amt)
    {
        currentScore -= amt;
    }

    public void SetSCore(int amt)
    {
        currentScore = amt;
    }
}
