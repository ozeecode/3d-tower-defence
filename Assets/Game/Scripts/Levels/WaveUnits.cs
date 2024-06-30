using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WaveUnits
{
    public UnitAI[] enemyPf;
    [Range(0, 100)] public int count;
    [SerializeField, Range(0f, 10f)] public float spawnInterval = 1.0f;

    public UnitAI GetUnitPf()
    {
        return enemyPf[Random.Range(0, enemyPf.Length)];
    }
}