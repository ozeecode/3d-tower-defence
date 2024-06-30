using DG.Tweening;
using KBCore.Refs;
using TMPro;
using UnityEngine;

public class TowerUI : MonoBehaviour
{
    [SerializeField, Child] private SlicedFilledImage fillImage;

    [SerializeField, Child] private TMP_Text towerHealthText;
    private void OnEnable()
    {
        EventManager.Subscribe<GameEvents.OnTowerHealthChange>(OnTowerHealthChange);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<GameEvents.OnTowerHealthChange>(OnTowerHealthChange);
    }
    private void OnTowerHealthChange(GameEvents.OnTowerHealthChange change)
    {

        towerHealthText.text = $"Tower {change.Health} / {change.MaxHealth}";

        towerHealthText.rectTransform.DOKill();
        towerHealthText.rectTransform.localScale = Vector3.one;
        towerHealthText.rectTransform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), .1f);

        fillImage.fillAmount = (float)change.Health / change.MaxHealth;
    }
}
