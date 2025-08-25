using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int score;
    public bool isFinish = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField] private float maxTime = 120f;

    [SerializeField] private float curTime;

    public void StartTimer()
    {
        StopAllCoroutines();
        curTime = maxTime;
        isFinish = false;
        StartCoroutine(OrderTimer());
    }

    public void StopTimer()
    {
        StopCoroutine("OrderTimer");
    }

    IEnumerator OrderTimer()
    {
        curTime = maxTime;
        while (curTime > 0)
        {
            curTime -= Time.deltaTime;

            if (curTime <= 0)
            {
                curTime = 0;
                isFinish = true;
                UIManager.instance.SetState(UIManager.State.End);
                yield break;
            }
            yield return null;
        }
    }

    public float GetTime()
    {
        return curTime;
    }

    public float GetMaxTime()
    {
        return maxTime;
    }

    public string GetScore()
    {
        return score.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
