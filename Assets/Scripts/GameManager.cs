using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider timeBar;
    public Canvas start;
    public Canvas inGame;
    public Canvas finishGame;
    public GameObject palette;

    private float time = 120f;

    private float curTime;

    public void Timer()
    {
        StartCoroutine(OrderTimer());
    }

    IEnumerator OrderTimer()
    {
        curTime = time;
        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            timeBar.value = curTime / time;
            yield return null;

            if (curTime <= 0)
            {
                curTime = 0;
                start.gameObject.SetActive(false);
                inGame.gameObject.SetActive(false);
                finishGame.gameObject.SetActive(true);
                yield break;
            }
        }
    }
}
