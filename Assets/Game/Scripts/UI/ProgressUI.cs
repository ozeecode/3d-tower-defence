using DG.Tweening;
using KBCore.Refs;
using TMPro;
using UnityEngine;


public class ProgressUI : MonoBehaviour
{
    [SerializeField, Anywhere] private TMP_Text totalWaveText;
    [SerializeField, Anywhere] private TMP_Text unitsLeftText;

    [SerializeField, Anywhere] private SlicedFilledImage totalWaveFill;
    [SerializeField, Anywhere] private SlicedFilledImage unitsLeftFill;



    private void OnEnable()
    {
        EventManager.Subscribe<GameEvents.OnWaveStart>(OnWaveStart);
        EventManager.Subscribe<GameEvents.OnEnemyCountChange>(OnEnemyCountChange);

    }


    private void OnDisable()
    {
        EventManager.Unsubscribe<GameEvents.OnWaveStart>(OnWaveStart);
        EventManager.Unsubscribe<GameEvents.OnEnemyCountChange>(OnEnemyCountChange);
    }

    private void OnWaveStart(GameEvents.OnWaveStart waveInfo)
    {

        UpdateWave(waveInfo.CurrentWaveIndex + 1, waveInfo.TotalWaveCount);
    }

    private void OnEnemyCountChange(GameEvents.OnEnemyCountChange info)
    {
        UpdateUnits(info.EnemyCount, info.MaxEnemyCount);
    }


    public void UpdateWave(int currentWave, int totalWave)
    {
        if (currentWave == totalWave)
        {
            totalWaveText.text = "Final Wave!";

        }
        else
        {
            totalWaveText.text = $"Wave {currentWave} / {totalWave}";

        }

        totalWaveText.rectTransform.DOKill();
        totalWaveText.rectTransform.localScale = Vector3.one;
        totalWaveText.rectTransform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), .1f);

        totalWaveFill.fillAmount = (float)currentWave / totalWave;
    }


    public void UpdateUnits(int unitsLeft, int totalUnits)
    {
        unitsLeftText.text = $"Units Left {unitsLeft}";

        unitsLeftText.rectTransform.DOKill();
        unitsLeftText.rectTransform.localScale=Vector3.one;
        unitsLeftText.rectTransform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), .1f);

        unitsLeftFill.fillAmount = (float)(totalUnits - unitsLeft) / totalUnits;
    }

}