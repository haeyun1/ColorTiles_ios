using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UIElements;

public class Title : MonoBehaviour, IUIState
{
    private UIDocument uI;
    List<Button> btns = new List<Button>();
    VisualElement popUp;
    public void Enter()
    {
        gameObject.SetActive(true);
        uI = gameObject.GetComponent<UIDocument>();
        var root = uI.rootVisualElement;
        btns.AddRange(root.Q<VisualElement>().Query<Button>().ToList());
        popUp = root.Q<VisualElement>("Overlay");
        btns.Add(popUp.Q<VisualElement>().Q<Button>());
        UIManager.instance.Hide(popUp);
        foreach (var btn in btns)
        {
            btn.clicked += () => OnClick(btn.name);
        }
    }

    void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "PlayBtn":
                UIManager.instance.SetState(UIManager.State.InGame);
                break;
            case "HelpBtn":
                UIManager.instance.Show(popUp);
                break;
            case "BackBtn":
                UIManager.instance.Hide(popUp);
                break;
            case "QuitBtn":
                GameManager.instance.QuitGame();
                break;
        }
    }
    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
