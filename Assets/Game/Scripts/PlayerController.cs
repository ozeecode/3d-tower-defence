using KBCore.Refs;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Scene] private Camera cam;
    [SerializeField] private LayerMask turretLayer;

    ISelectable currentSelected;
    private void OnEnable()
    {
        InputManager.TouchStartEvent += OnTouchStart;
    }

    private void OnTouchStart(Vector2 touchPoint)
    {
        if (InputManager.IsPointerOverUI) return;
        if (currentSelected is not null)
        {
            currentSelected.Deselect();
            currentSelected = null;
            return;
        }
        if (Utils.TryGetTouchInfo(touchPoint, out ISelectable selection, turretLayer, cam) && selection.IsActive)
        {
            currentSelected = selection;
            selection.Select();
        }
    }
}
