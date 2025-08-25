using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    List<UIDocument> uIDocuments;

    private IUIState state;

    public enum State
    {
        Title,
        Help,
        InGame,
        End
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetState(State.Title);
    }

    public void SetState(State state)
    {
        this.state?.Exit();
        var doc = uIDocuments[(int)state];
        this.state = doc.GetComponent<IUIState>();
        this.state.Enter();
    }

    public void Show(VisualElement element)
    {
        element.RemoveFromClassList("hidden");
    }

    public void Hide(VisualElement element)
    {
        element.AddToClassList("hidden");
    }
}
