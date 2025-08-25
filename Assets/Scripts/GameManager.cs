using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Slider timeBar;
    public Canvas start;
    public Canvas inGame;
    public Canvas finishGame;
    public bool isFinish = false;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private float time = 120f;

    private float curTime;

    public void Timer()
    {
        StopAllCoroutines();
        curTime = time;
        isFinish = false;
        StartCoroutine(OrderTimer());
    }

    IEnumerator OrderTimer()
    {
        curTime = time;
        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            timeBar.value = curTime / time;

            if (curTime <= 0)
            {
                curTime = 0;
                start.gameObject.SetActive(false);
                inGame.gameObject.SetActive(false);
                finishGame.gameObject.SetActive(true);
                isFinish = true;
                break;
            }

            yield return null;
        }
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
