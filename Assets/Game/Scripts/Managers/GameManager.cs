using KBCore.Refs;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField, Scene] private LevelManager levelManager;
    [SerializeField, Scene] private Tower tower;
    [SerializeField, Scene] private CurrencyController currencyController;

    private GameData gameData;

    private void Awake()
    {
        gameData = GameData.Load();
        if (gameData is null)
        {
            SetDefaultData();
        }
    }

    private void OnEnable()
    {
        EventManager.Subscribe<GameEvents.OnSave>(OnSave);
        EventManager.Subscribe<GameEvents.OnPlayClick>(OnPlay);
        EventManager.Subscribe<GameEvents.OnDeleteProgress>(OnDeleteProgress);

        EventManager.Subscribe<GameEvents.OnLevelCompleted>(OnLevelCompleted);
        EventManager.Subscribe<GameEvents.OnLevelFailed>(OnLevelFailed);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<GameEvents.OnSave>(OnSave);
        EventManager.Unsubscribe<GameEvents.OnPlayClick>(OnPlay);
        EventManager.Unsubscribe<GameEvents.OnDeleteProgress>(OnDeleteProgress);

        EventManager.Unsubscribe<GameEvents.OnLevelCompleted>(OnLevelCompleted);
        EventManager.Unsubscribe<GameEvents.OnLevelFailed>(OnLevelFailed);
    }

    private void Start() => EventManager.Fire(new GameEvents.OnLoadCompleted(gameData.Level));
    private void OnLevelFailed(GameEvents.OnLevelFailed failed) => Save();
    private void OnPlay(GameEvents.OnPlayClick click) => Load();
    private void OnLevelCompleted(GameEvents.OnLevelCompleted completed) => Save();
    private void OnSave(GameEvents.OnSave save) => Save();

    private void OnDeleteProgress(GameEvents.OnDeleteProgress progress)
    {
        DeleteProgress();
        Load();
    }

    private void Load()
    {
        tower.SetTurretDatas(gameData.TurretDatas);
        levelManager.Init(gameData.Level);
        currencyController.SetCoin(gameData.Coin);

        EventManager.Fire(new GameEvents.OnGameStart());
        InputManager.Instance.SetGameInput(true);
    }


#if UNITY_EDITOR
    [ContextMenu("DeleteProgress")]
    private void ForceDelete()
    {
        SaveIO.SerioslyDeleteAllFiles();
    }
#endif


    private void DeleteProgress()
    {


        SaveIO.SerioslyDeleteAllFiles();
        SetDefaultData();
    }

    private void Save()
    {
        TurretSaveData[] turretSaveDatas = tower.GetTurretSaveDatas();
        long currentGold = currencyController.Coin;
        int level = levelManager.Level;

        gameData = new GameData(turretSaveDatas, level, currentGold);
        gameData.Save();
    }

    private void SetDefaultData()
    {
        TurretSaveData[] turretSaveData = new TurretSaveData[tower.TurretCount];
        for (int i = 0; i < turretSaveData.Length; i++)
        {
            turretSaveData[i] = new TurretSaveData { Level = i != 0 ? -1 : 0, ProjectileType = 0 };
        }
        gameData = new GameData(turretSaveData, 0, 0);
    }


}
