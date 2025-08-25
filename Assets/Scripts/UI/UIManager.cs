using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    List<UIDocument> uIDocuments;

    [SerializeField]
    GameObject tile;

    [SerializeField] bool iOS = true; // 에디터 테스트용

    private IUIState state;

    public enum State
    {
        Title,
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

    public void ApplyPlatformStyles(UIDocument uI)
    {
        var root = uI.rootVisualElement;

        // 1) 공통 스타일
        var baseStyle = Resources.Load<StyleSheet>("Styles/Common");
        if (baseStyle != null)
            root.styleSheets.Add(baseStyle);

        // 2) 플랫폼별 스타일
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            (Application.isEditor && iOS))
        {
            var iosStyle = Resources.Load<StyleSheet>("Styles/iOS");
            if (iosStyle != null)
                root.styleSheets.Add(iosStyle);
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer ||
                 Application.platform == RuntimePlatform.OSXPlayer ||
                 (Application.isEditor && !iOS))
        {
            var pcStyle = Resources.Load<StyleSheet>("Styles/PC");
            if (pcStyle != null)
                root.styleSheets.Add(pcStyle);
        }

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

    public void ShowTile(bool isActive)
    {
        tile.SetActive(isActive);
    }
}
