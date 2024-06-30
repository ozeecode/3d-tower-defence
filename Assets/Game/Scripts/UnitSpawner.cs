using KBCore.Refs;
using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField, Anywhere] private Transform[] spawnPoints;
    [SerializeField, Anywhere] private UnitAI archerPf;
    [SerializeField, Anywhere] private UnitAI meleePf;


    private List<UnitAI> spawnedUnits = new List<UnitAI>();


    private void OnEnable()
    {
        EventManager.Subscribe<GameEvents.OnLevelLoaded>(OnLevelLoaded);
    }

    private void OnLevelLoaded(GameEvents.OnLevelLoaded loaded)
    {
        foreach (UnitAI unit in spawnedUnits)
        {
            LeanPool.Despawn(unit);
        }
        spawnedUnits.Clear();
    }

    internal void AddArcher()
    {
        long archerCost = CurrencyController.Instance.GetArcherCost();
        if (CurrencyController.Instance.CanSpendCoin(archerCost))
        {
            CurrencyController.Instance.SpendCoin(archerCost);
            SpawnArcher();
        }
    }

    internal void AddMelee()
    {
        long meleeCost = CurrencyController.Instance.GetMeleeCost();
        if (CurrencyController.Instance.CanSpendCoin(meleeCost))
        {
            CurrencyController.Instance.SpendCoin(meleeCost);
            SpawnMelee();
        }
    }

    private void SpawnMelee()
    {
        SpawnUnit(meleePf);
    }

    private void SpawnArcher()
    {
        SpawnUnit(archerPf);
    }

    private void SpawnUnit(UnitAI unitPf)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        UnitAI unit = LeanPool.Spawn(unitPf, spawnPoint.position, Quaternion.identity);
        spawnedUnits.Add(unit);
    }
}


