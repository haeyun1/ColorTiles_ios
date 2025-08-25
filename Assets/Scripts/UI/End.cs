using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class End : MonoBehaviour, IUIState
{
    private UIDocument uI;
    Label score;
    Button homeBtn;
    public void Enter()
    {
        gameObject.SetActive(true);
        uI = gameObject.GetComponent<UIDocument>();

        var root = uI.rootVisualElement;
        score = root.Q<Label>("Score");
        score.text = GameManager.instance.GetScore();

        homeBtn = root.Q<Button>();
        homeBtn.clicked += OnClick;
    }

    void OnClick()
    {
        UIManager.instance.SetState(UIManager.State.Title);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
