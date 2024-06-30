using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Character Data")]
public class CharacterDataSO : ScriptableObject
{
    public LayerMask targetLayer;
    public Color Color;
    public int MaxHealth = 100;
    public Vector3 TargetPosition;
    [Range(0, 5)] public float MoveSpeed = 5f;
    [Range(0, 5)] public float RotationSpeed = 1f;
    [Range(0, 100)] public float RadarRange = 1f;
    [Range(0, 5)] public float AttackRange = 1f;
    [Range(0, 5)] public float AttackDelay = 1f;
    [Range(0, 1000)] public int Damage = 1;
}