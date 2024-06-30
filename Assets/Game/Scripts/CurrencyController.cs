using UnityEngine;

public class CurrencyController : SingletonMonoBehaviour<CurrencyController>
{
    public long Coin { get; private set; }

    [SerializeField] private int killReward = 1;
    [SerializeField] private int turretBaseCost = 10;
    [SerializeField] private int archerBaseCost = 3;
    [SerializeField] private int meleeBaseCost = 2;

    public bool CanSpendCoin(long amount) => amount <= Coin;


    public void SpendCoin(long amount)
    {
        Coin -= amount;
        EventManager.Fire(new GameEvents.OnCoinChange(Coin));
    }

    internal void SetCoin(long coin)
    {
        Coin = coin;
        EventManager.Fire(new GameEvents.OnCoinChange(Coin));
    }
    public void AddCoin(long coin)
    {
        Coin += coin;
        EventManager.Fire(new GameEvents.OnCoinChange(Coin));
    }


    public void EnemyKilled()
    {
        AddCoin(killReward);
    }

    public long GetTurretCost(int activatedTurretCount)
    {
        return (activatedTurretCount * turretBaseCost) + turretBaseCost;
    }
    internal bool CanSpendTurretCost(int activatedTurretCount, out long turretCost)
    {
        turretCost = GetTurretCost(activatedTurretCount);
        return CanSpendCoin(turretCost);
    }

    internal int GetArcherCost()
    {
        return archerBaseCost;
    }

    internal int GetMeleeCost()
    {
        return meleeBaseCost;
    }

    internal long GetTurretUpgradeCost(int level)
    {
        return (level * turretBaseCost) + turretBaseCost;
    }
}
