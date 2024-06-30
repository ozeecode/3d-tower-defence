using KBCore.Refs;
using System.Linq;
using UnityEngine;


public class Tower : MonoBehaviour, IDamageable
{
    public Vector3 Position => transform.position;
    public bool IsAlive => currentHealth > 0;
    public Transform DamagePoint => damagePoint;
    public int TurretCount => turrets.Length;
    public bool CanAddTurret => ActivatedTurretCount < TurretCount;
    public int ActivatedTurretCount => turrets.Count(turret => turret.Level >= 0);

    [SerializeField, Child] private Turret[] turrets;
    [SerializeField] private int MaxHealth;
    [SerializeField, Anywhere] private Transform damagePoint;

    private int currentHealth;

    private void OnEnable()
    {
        EventManager.Subscribe<GameEvents.OnLevelLoaded>(OnLevelLoaded);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe<GameEvents.OnLevelLoaded>(OnLevelLoaded);
    }
    private void OnLevelLoaded(GameEvents.OnLevelLoaded OnLevelLoaded)
    {
        currentHealth = MaxHealth;
        foreach (var t in turrets)
        {
            t.enabled = true;
        }
        EventManager.Fire(new GameEvents.OnTowerHealthChange(currentHealth, MaxHealth));
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }

        EventManager.Fire(new GameEvents.OnTowerHealthChange(currentHealth, MaxHealth));
    }

    private void Death()
    {
        Debug.Log("Destruction!");
        EventManager.Fire(new GameEvents.OnLevelFailed());
        foreach (var t in turrets)
        {
            t.enabled = false;
        }
    }



    internal TurretSaveData[] GetTurretSaveDatas()
    {
        TurretSaveData[] data = new TurretSaveData[turrets.Length];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new TurretSaveData { Level = turrets[i].Level, ProjectileType = 0 };
        }
        return data;
    }

    internal void SetTurretDatas(TurretSaveData[] turretDatas)
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            turrets[i].Init(turretDatas[i].Level, (ProjectileType)turretDatas[i].ProjectileType);
        }
    }

    internal bool TryAddTurret()
    {

        if (CurrencyController.Instance.CanSpendTurretCost(ActivatedTurretCount, out long turretCost))
        {
            for (int i = 0; i < turrets.Length; i++)
            {
                if (turrets[i].Level >= 0) continue;
                CurrencyController.Instance.SpendCoin(turretCost);
                turrets[i].Init(0, ProjectileType.Bullet);
                return true;
            }
        }
        return false;
    }
}
