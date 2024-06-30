using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public abstract class PanelUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public bool IsVisibleOnStart = false;
    public bool DisableGameInputWhenVisible;
    public bool EnableGameInputWhenVisible;
    protected virtual void OnValidate()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    protected virtual void Start()
    {
        if (IsVisibleOnStart)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public virtual void Show()
    {
        if (DisableGameInputWhenVisible)
        {
            InputManager.Instance.SetGameInput(false);
        }
        if (EnableGameInputWhenVisible)
        {
            InputManager.Instance.SetGameInput(true);
        }
        SetState(true);
    }

    public virtual void Hide()
    {

        SetState(false);
    }



    protected virtual void SetState(bool isEnabled)
    {
        canvasGroup.alpha = isEnabled ? 1 : 0;
        canvasGroup.interactable = isEnabled;
        canvasGroup.blocksRaycasts = isEnabled;
    }
}