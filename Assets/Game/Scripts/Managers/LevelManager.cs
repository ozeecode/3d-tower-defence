using Cysharp.Threading.Tasks;
using KBCore.Refs;
using Lean.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



public class LevelManager : MonoBehaviour
{
    public int Level { get; private set; }


    [SerializeField, Scene] private WaveController waveController;
    [SerializeField, Anywhere] private LevelDataSO[] levels;
    [SerializeField, Anywhere] private Transform[] spawnPoints;



    private List<UnitAI> spawnedList = new List<UnitAI>();

    private int totalUnitCount, leftUnitCount;
    public void Init(int level)
    {
        Level = level;
        LoadLevel();
    }

    private void OnEnable()
    {
        EventManager.Subscribe<GameEvents.OnNextLevel>(OnNextLevel);
        EventManager.Subscribe<GameEvents.OnLevelRestart>(OnLevelRestart);
        EventManager.Subscribe<GameEvents.OnEnemyDeath>(OnEnemyDeath);
    }


    private void OnDisable()
    {
        EventManager.Unsubscribe<GameEvents.OnNextLevel>(OnNextLevel);
        EventManager.Unsubscribe<GameEvents.OnLevelRestart>(OnLevelRestart);
        EventManager.Unsubscribe<GameEvents.OnEnemyDeath>(OnEnemyDeath);
    }

    private void OnNextLevel(GameEvents.OnNextLevel level) => LoadLevel();
    private void OnLevelRestart(GameEvents.OnLevelRestart restart) => LoadLevel();

    private void LoadLevel()
    {
        ClearSpawnedUnits();
        int levelIndex = Level % levels.Length;
        LevelDataSO currentLevelData = levels[levelIndex];

        EventManager.Fire(new GameEvents.OnLevelLoaded(Level, currentLevelData.Waves.Length));
        WaveSpawner(currentLevelData);
    }

    private void ClearSpawnedUnits()
    {
        foreach (var unit in spawnedList)
        {
            LeanPool.Despawn(unit);
        }
        spawnedList.Clear();
    }
    //private void NextLevel()
    //{
    //    Level++;
    //    LoadLevel();
    //}

    #region Wave&Spawner Methods
    private void OnEnemyDeath(GameEvents.OnEnemyDeath obj)
    {
        if (spawnedList.Remove(obj.Enemy))
        {
            leftUnitCount--;
            CurrencyController.Instance.EnemyKilled();
            EventManager.Fire(new GameEvents.OnEnemyCountChange(leftUnitCount, totalUnitCount));
        }
    }

    private async void WaveSpawner(LevelDataSO currentLevelData)
    {
        int totalWaveCount = currentLevelData.Waves.Length;

        for (int i = 0; i < totalWaveCount; i++)
        {
            WaveData wave = currentLevelData.Waves[i];
            totalUnitCount = wave.TotalUnitCount;
            leftUnitCount = totalUnitCount;

            EventManager.Fire(new GameEvents.OnWaveStart(i, totalWaveCount, totalUnitCount));
            EventManager.Fire(new GameEvents.OnEnemyCountChange(leftUnitCount, totalUnitCount));

            foreach (WaveUnits waveUnit in wave.Units)
            {
                for (int j = 0; j < waveUnit.count; j++)
                {
                    SpawnUnit(waveUnit.GetUnitPf());
                    await UniTask.Delay(GetRandomSpawnInterval(waveUnit.spawnInterval));
                }
            }

            await UniTask.WaitUntil(() => spawnedList.Count == 0);

            await UniTask.Delay(TimeSpan.FromSeconds(wave.delay));
            //Wave end!
        }
        Level++; //save icin quick fix daha iyi bir cozum bulamadým (:
        EventManager.Fire(new GameEvents.OnLevelCompleted());
    }


    private void SpawnUnit(UnitAI unitPf)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        UnitAI unit = LeanPool.Spawn(unitPf, spawnPoint.position, Quaternion.identity);
        spawnedList.Add(unit);
    }
    private TimeSpan GetRandomSpawnInterval(float spawnInterval)
    {
        return TimeSpan.FromSeconds(Random.Range(0f, spawnInterval));
    }
    #endregion
}
