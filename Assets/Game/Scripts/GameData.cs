using System;

[Serializable]
public class GameData
{
    private static readonly string SAVE_FILE = "SaveData";

    public TurretSaveData[] TurretDatas;
    public int Level;
    public long Coin;
    //public static GameData Curret { get; private set; }
    public GameData(TurretSaveData[] turretDatas, int level, long coin)
    {
        TurretDatas = turretDatas;
        Level = level;
        Coin = coin;
    }

    public void Save()
    {
        SaveIO.SaveData(this, SAVE_FILE);
    }


    public static GameData Load()
    {
        return SaveIO.LoadData<GameData>(SAVE_FILE);

    }
}