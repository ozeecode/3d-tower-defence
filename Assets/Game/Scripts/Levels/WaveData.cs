using System;
using UnityEngine;

[Serializable]
public class WaveData
{
    public WaveUnits[] Units;
    [Range(0, 60f)] public float delay;
    public int TotalUnitCount;

    public void CalcTotalUnit()
    {
        TotalUnitCount = 0;
        foreach (var unit in Units)
        {
            TotalUnitCount += unit.count;
        }
    }
}
