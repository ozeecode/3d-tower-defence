using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance;
    private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin perlin;

    public float intensity = 1f;
    public float duration = .2f;

    bool isCoroutineRunning;
    Coroutine shakerRoutine;
    private void Awake()
    {
        Instance = this;
        vCam = GetComponent<CinemachineVirtualCamera>();
        perlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera()
    {
        if (isCoroutineRunning)
        {
            StopCoroutine(shakerRoutine);
        }
        perlin.m_AmplitudeGain = intensity;
        shakerRoutine = StartCoroutine(Shaker());
    }

    private IEnumerator Shaker()
    {
        float timer = 0;
        float startingIntensity = intensity;
        isCoroutineRunning = true;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            perlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, t);
            yield return null;
        }
        isCoroutineRunning = false;
    }


}