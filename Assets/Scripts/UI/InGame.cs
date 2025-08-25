using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InGame : MonoBehaviour, IUIState
{
    private UIDocument uI;
    Button homeBtn;
    ProgressBar time;
    Label score;

    float maxTime;
    public void Enter()
    {
        gameObject.SetActive(true);
        uI = gameObject.GetComponent<UIDocument>();

        var root = uI.rootVisualElement;
        homeBtn = root.Q<Button>();
        homeBtn.clicked += OnClick;
        time = root.Q<ProgressBar>();
        score = root.Q<Label>("Score");

        GameManager.instance.ResetScore();
        maxTime = GameManager.instance.GetMaxTime();
        GameManager.instance.StartTimer();
        UIManager.instance.ShowTile(true);
    }

    void OnClick()
    {
        UIManager.instance.SetState(UIManager.State.Title);
    }

    void Update()
    {
        score.text = GameManager.instance.GetScore();
        time.value = GameManager.instance.GetTime() / maxTime;
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        GameManager.instance.StopTimer();
    }
}
