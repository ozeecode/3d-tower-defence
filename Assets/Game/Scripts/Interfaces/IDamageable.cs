using UnityEngine;

public interface IDamageable
{
    public Transform DamagePoint { get; }
    public Vector3 Position { get; }
    public bool IsAlive { get; }
    public void TakeDamage(int damage);
}
