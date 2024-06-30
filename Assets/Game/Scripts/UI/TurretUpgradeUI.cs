using DG.Tweening;
using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class TurretUpgradeUI : PanelUI
{
    [SerializeField, Self] private RectTransform rectTransform;
    [SerializeField] private Ease ease;

    [SerializeField, Anywhere] private TMP_Text levelText;
    [SerializeField, Anywhere] private TMP_Text damageText;
    [SerializeField, Anywhere] private TMP_Text fireDelayText;
    [SerializeField, Anywhere] private TMP_Text upgradeCostText;
    [SerializeField, Anywhere] private Button upgardeButton;

    [SerializeField] private float showY = -100;
    [SerializeField] private float hideY = -1000;

    private IUpgradeable currentUpgradeable;
    private void OnEnable()
    {
        EventManager.Subscribe<GameEvents.OnLevelCompleted>(OnLevelCompleted);
        EventManager.Subscribe<GameEvents.OnLevelFailed>(OnLevelFailed);
        EventManager.Subscribe<GameEvents.OnTurretSelected>(OnTurretSelected);
        EventManager.Subscribe<GameEvents.OnCameraChange>(OnCameraChange);
    }
    private void OnTurretSelected(GameEvents.OnTurretSelected selected)
    {
        Show();
        currentUpgradeable = selected.Turret;
        SetUpgaradeableInfo();
    }
    private void OnLevelCompleted(GameEvents.OnLevelCompleted completed) => Hide();
    private void OnLevelFailed(GameEvents.OnLevelFailed failed) => Hide();
    private void OnCameraChange(GameEvents.OnCameraChange change)
    {
        if (change.Target == null)
        {
            Hide();
        }
    }

    public override void Show()
    {
        rectTransform.DOKill();
        base.Show();
        rectTransform.DOMoveY(showY, 1f).SetEase(ease);
    }

    public override void Hide()
    {
        rectTransform.DOKill();
        rectTransform.DOMoveY(hideY, 1f).SetEase(ease).OnComplete(() =>
        {
            base.Hide();
        });

    }


    private void SetUpgaradeableInfo()
    {

        levelText.SetText((currentUpgradeable.Level + 1).ToString());
        damageText.SetText(currentUpgradeable.Damage.ToString());
        fireDelayText.SetText(currentUpgradeable.AttackDelay.ToString() + " sec.");

        long upgradeCost = CurrencyController.Instance.GetTurretUpgradeCost(currentUpgradeable.Level);

        upgardeButton.interactable = CurrencyController.Instance.CanSpendCoin(upgradeCost);
        upgradeCostText.SetText(upgradeCost.ToString());
    }

    public void OnUpgradeClick()
    {
        currentUpgradeable.Upgrade();
        SetUpgaradeableInfo();

    }


}
