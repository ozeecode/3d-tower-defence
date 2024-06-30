using Cinemachine;
using System;
using UnityEngine;


public class CamController : MonoBehaviour
{
    //Dictionary<GameObject, CinemachineVirtualCamera> camDict = new Dictionary<GameObject, CinemachineVirtualCamera>();

    [SerializeField] private CamPair[] camPairs;


    //private void Awake()
    //{
    //    foreach (var pair in camPairs)
    //    {
    //        camDict.Add(pair.target, pair.vCam);
    //    }
    //}


    private void OnEnable()
    {
        EventManager.Subscribe<GameEvents.OnLevelCompleted>(OnLevelCompleted);
        EventManager.Subscribe<GameEvents.OnLevelFailed>(OnLevelFailed);

        EventManager.Subscribe<GameEvents.OnCameraChange>(CameraChange);
    }

    private void OnLevelFailed(GameEvents.OnLevelFailed failed)
    {
        SetTurretCamera(null);
    }

    private void OnLevelCompleted(GameEvents.OnLevelCompleted completed)
    {
        SetTurretCamera(null);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<GameEvents.OnCameraChange>(CameraChange);
    }
    private void CameraChange(GameEvents.OnCameraChange change)
    {
        SetTurretCamera(change.Target);
    }

    public void SetTurretCamera(GameObject target)
    {
        foreach (var pair in camPairs)
        {
            pair.vCam.enabled = pair.target == target;
        }
    }
}

[Serializable]
public struct CamPair
{
    public GameObject target;
    public CinemachineVirtualCamera vCam;
}