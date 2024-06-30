using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Turret Data")]
public class TurretDataSO : ScriptableObject
{
    public LayerMask targetLayer;
    [Range(0, 5)] public float RotationSpeed = 1f;
    [Range(0, 50)] public float AttackRange = 1f;
    [Range(0, 1)] public float AttackDelay = 1f;
    [Range(0, 1000)] public int Damage = 1;
    public ProjectileBehaviour Projectile;

    //public int Level = 1;
}
