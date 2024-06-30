using Cysharp.Threading.Tasks;
using UnityEngine;

public class AttackState : BaseState
{
    private static readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private CharacterDataSO characterData;
    private Animator animator;
    private float nextAttackTime;
    public AttackState(UnitAI character, Animator animator, CharacterDataSO characterData) : base(character)
    {
        this.characterData = characterData;
        this.animator = animator;
    }

    public override void Tick()
    {
        if (Time.realtimeSinceStartup < nextAttackTime)
        {
            return;
        }
        Attack();
    }

    private async void Attack()
    {
        nextAttackTime = Time.realtimeSinceStartup + characterData.AttackDelay;
        animator.SetTrigger(ATTACK_HASH);
        await UniTask.Delay(500);
        if (character.Target is null || !character.Target.IsAlive)
        {
            return;
        }
        character.Target.TakeDamage(characterData.Damage);
    }
}
