using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_instance = null;
    private static bool isInit = false;
    public static T Instance
    {
        get
        {
            if (!isInit)
            {
                s_instance = FindObjectOfType<T>();
                if (s_instance == null)
                {
                    GameObject container = new GameObject(typeof(T).FullName);
                    s_instance = container.AddComponent<T>();
                }
                isInit = true;
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
#endif
                    s_instance.transform.SetParent(null);
                    DontDestroyOnLoad(s_instance);
#if UNITY_EDITOR
                }
#endif
            }
            return s_instance;
        }
    }

}

