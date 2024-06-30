using Cysharp.Threading.Tasks;
using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameUI : PanelUI
{
    [SerializeField, Scene] private Tower tower;
    [SerializeField, Scene] private UnitSpawner unitSpawner;
    [SerializeField, Anywhere] private Button addArcherButton;
    [SerializeField, Anywhere] private Button addMeleeButton;
    [SerializeField, Anywhere] private Button addTurretButton;

    [SerializeField, Anywhere] private TMP_Text goldText;
    [SerializeField, Anywhere] private TMP_Text levelText;

    [SerializeField, Anywhere] private TMP_Text archerCostText;
    [SerializeField, Anywhere] private TMP_Text meleeCostText;
    [SerializeField, Anywhere] private TMP_Text turretCostText;

    private long goldAmount;

    private async void OnEnable()
    {
        EventManager.Subscribe<GameEvents.OnCoinChange>(OnCoinChange);
        EventManager.Subscribe<GameEvents.OnLevelLoaded>(OnLevelLoaded);

        await UniTask.NextFrame();

        SetButtonState();
    }


    private void OnDisable()
    {
        EventManager.Unsubscribe<GameEvents.OnCoinChange>(OnCoinChange);
        EventManager.Unsubscribe<GameEvents.OnLevelLoaded>(OnLevelLoaded);
    }
    private void OnLevelLoaded(GameEvents.OnLevelLoaded loaded)
    {
        Show();
        SetButtonState();
        levelText.SetText($"Level {loaded.LevelIndex + 1}");
    }

    private void OnCoinChange(GameEvents.OnCoinChange change)
    {
        goldAmount = change.Amount;
        goldText.SetText(goldAmount.ToString());
        SetButtonState();
    }

    public void OnAddTurretClick()
    {
        if (tower.TryAddTurret())
        {
            //SetButtonState(); // gerek yok zaten coin miktari degisince tetikleniyor!
        }
    }

    public void OnAddArcherClick()
    {
        unitSpawner.AddArcher();
    }
    public void OnAddMeleeClick()
    {
        unitSpawner.AddMelee();
    }

    private void SetButtonState()
    {


        long archerCost = CurrencyController.Instance.GetArcherCost();
        addArcherButton.interactable = CurrencyController.Instance.CanSpendCoin(archerCost);
        archerCostText.SetText(archerCost.ToString());

        long meleeCost = CurrencyController.Instance.GetMeleeCost();
        addMeleeButton.interactable = CurrencyController.Instance.CanSpendCoin(meleeCost);
        meleeCostText.SetText(meleeCost.ToString());

        if (!tower.CanAddTurret)
        {
            addTurretButton.gameObject.SetActive(false);
        }
        else
        {
            addTurretButton.gameObject.SetActive(true);
            //turret button 
            long turretCost = CurrencyController.Instance.GetTurretCost(tower.ActivatedTurretCount);
            turretCostText.text = turretCost.ToString();
            addTurretButton.interactable = CurrencyController.Instance.CanSpendCoin(turretCost);
        }



    }
}

