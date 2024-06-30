using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level Data")]
public class LevelDataSO : ScriptableObject
{
    public WaveData[] Waves;

    private void OnValidate()
    {
        foreach (WaveData wave in Waves)
        {
            wave.CalcTotalUnit();
        }
    }
}
