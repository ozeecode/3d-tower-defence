using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    public static bool IsPointerOverUI { get; private set; }
    public static event Action<Vector2> TouchStartEvent;
    //public static event Action<Vector2> RightClickEvent;
    public static event Action TouchEndEvent;
    private PlayerInputActions inputActions;

    public static bool IsTouching => isTouching;
    public static Vector3 TouchPoint => Instance.inputActions.Game.TouchPosition.ReadValue<Vector2>();

    static bool isTouching;




    private void OnEnable()
    {
        inputActions ??= new PlayerInputActions();
        //inputActions.Game.Enable();
        inputActions.UI.Enable();
        inputActions.Game.TouchState.performed += TouchState_performed;
        inputActions.Game.TouchState.canceled += TouchState_canceled;



        //inputActions.Game.RightClick.performed += RightClick_performed;

    }



    private void OnDisable()
    {
        if (inputActions is null) return;
        inputActions.Game.Disable();
        inputActions.Game.TouchState.performed -= TouchState_performed;
        inputActions.Game.TouchState.canceled -= TouchState_canceled;

        //inputActions.Game.RightClick.performed -= RightClick_performed;
    }


    private void Update()
    {
        if (EventSystem.current != null)
        {
            IsPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        }
    }

    public void SetUI(bool isEnabled)
    {
        if (isEnabled)
        {
            inputActions.UI.Enable();
        }
        else
        {
            inputActions.UI.Disable();
        }
    }

    public void SetGameInput(bool isEnabled)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif
        if (isEnabled)
        {
            inputActions.Game.Enable();
        }
        else
        {
            inputActions?.Game.Disable();
        }
    }

    //private void RightClick_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    //{
    //    RightClickEvent?.Invoke(TouchPoint);
    //}
    private void TouchState_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isTouching = false;
        TouchEndEvent?.Invoke();
    }

    private void TouchState_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isTouching = true;
        TouchStartEvent?.Invoke(TouchPoint);
    }
}