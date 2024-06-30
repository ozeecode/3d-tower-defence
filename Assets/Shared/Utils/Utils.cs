using UnityEngine;

public static class Utils
{
    public static bool TryGetTouchInfo<T>(Vector3 touchPoint, out T target, LayerMask layer, Camera cam)
    {
        Ray ray = cam.ScreenPointToRay(touchPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, cam.farClipPlane, layer) && hit.transform.TryGetComponent(out T t))
        {
            target = t;
            return true;
        }
        target = default;
        return false;
    }
    public static bool TryGetTouchInfoInParent<T>(Vector3 touchPoint, out T target, LayerMask layer, Camera cam)
    {
        Ray ray = cam.ScreenPointToRay(touchPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, cam.farClipPlane, layer) && hit.transform.parent.TryGetComponent(out T t))
        {
            target = t;
            return true;
        }
        target = default;
        return false;
    }



    /// <param name="message">Message string to show in the toast.</param>
    public static void ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }




}