using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text myScore;
    public TMP_Text myScore2;
    private int score = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
        myScore.text = score.ToString();
        myScore2.text = score.ToString();
        Debug.Log($"현재 점수: {score}");
    }

    public void ResetScore()
    {
        score = 0;
        myScore.text = score.ToString();
        myScore2.text = score.ToString();
    }
}
