using UnityEngine;

public class EnemyAI : UnitAI
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out IDamageable target) && target is not EnemyAI)
        {
            SetForcedTarget(target);
        }
    }
    protected override void Death()
    {
        base.Death();
        EventManager.Fire(new GameEvents.OnEnemyDeath(this));
    }

}