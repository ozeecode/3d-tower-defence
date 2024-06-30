using System;
using UnityEngine;


public static class EventManager
{
    static EventBus _eventBus;

    static EventManager()
    {
        Container.Initialize();
        _eventBus = ServiceLocator.Instance.Resolve<EventBus>();
    }

    public static void Subscribe<T>(Action<T> action) where T : IEvent
    {
        _eventBus.Subscribe(action);
    }

    public static void Unsubscribe<T>(Action<T> action) where T : IEvent
    {
        _eventBus.Unsubscribe(action);
    }

    public static void Fire<T>(T payload) where T : IEvent
    {
        _eventBus.Fire(payload);
    }
}

public class GameEvents : MonoBehaviour
{
    public struct OnTowerHealthChange : IEvent
    {
        public int MaxHealth;
        public int Health;

        public OnTowerHealthChange(int health, int maxHealth)
        {
            MaxHealth = maxHealth;
            Health = health;
        }
    }
    public struct OnEnemyCountChange : IEvent
    {
        public int EnemyCount;
        public int MaxEnemyCount;

        public OnEnemyCountChange(int enemyCount, int maxEnemyCount)
        {
            EnemyCount = enemyCount;
            MaxEnemyCount = maxEnemyCount;
        }
    }
    public struct OnEnemyDeath : IEvent
    {
        public UnitAI Enemy;
        public OnEnemyDeath(UnitAI enemy)
        {
            Enemy = enemy;
        }
    }

    public struct OnCameraChange : IEvent
    {
        public GameObject Target;
        public OnCameraChange(GameObject target)
        {
            Target = target;
        }
    }

    public struct OnTurretSelected : IEvent
    {
        public IUpgradeable Turret;
        public OnTurretSelected(IUpgradeable target)
        {
            Turret = target;
        }
    }


    public struct OnCoinChange : IEvent
    {
        public long Amount;
        public OnCoinChange(long amount)
        {
            Amount = amount;
        }
    }



    public struct OnLoadCompleted : IEvent
    {
        public int levelIndex;
        public OnLoadCompleted(int level)
        {
            levelIndex = level;
        }
    }
    public struct OnLevelLoaded : IEvent
    {
        public int LevelIndex;
        public int TotalWaveCount;
        public OnLevelLoaded(int level, int totalWaveCount)
        {
            LevelIndex = level;
            TotalWaveCount = totalWaveCount;
        }
    }


    public struct OnWaveStart : IEvent
    {
        public int CurrentWaveIndex;
        public int TotalWaveCount;
        public int TotalUnitCount;

        public OnWaveStart(int currentWave, int maxWave, int maxUnitCount)
        {
            CurrentWaveIndex = currentWave;
            TotalWaveCount = maxWave;
            TotalUnitCount = maxUnitCount;
        }
    }
    public struct OnSave : IEvent { }
    public struct OnPlayClick : IEvent { }
    public struct OnGameStart : IEvent { }
    public struct OnDeleteProgress : IEvent { }
    public struct OnNextLevel : IEvent { }
    public struct OnLevelFailed : IEvent { }
    public struct OnLevelCompleted : IEvent { }
    public struct OnLevelRestart : IEvent { }


}