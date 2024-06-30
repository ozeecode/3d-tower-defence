using Ozee.StateMachine;
using UnityEngine;

public class BomberEnemy : EnemyAI
{

    protected override void OnEnable()
    {
        base.OnEnable();
        Target = FindAnyObjectByType<Tower>();
    }
    protected override void StateMachineInit()
    {
        MoveState moveState = new MoveState(rb, this, characterData);
        BomberAttackState attackState = new BomberAttackState(this, characterData);
        DeathState deathState = new DeathState(this, rb);

        StateTransition[] transitions = new StateTransition[3];
        transitions[0] = new StateTransition(attackState, moveState, AttackToMoveCondition);
        transitions[1] = new StateTransition(moveState, attackState, MoveToAttackCondition);
        transitions[2] = new StateTransition(deathState, moveState, () => IsAlive);


        StateTransition[] anyTransitions = new StateTransition[1];
        anyTransitions[0] = new StateTransition(null, deathState, () => !IsAlive);


        stateMachine = new StateMachine(moveState, transitions, anyTransitions);
    }

    private bool AttackToMoveCondition() => Target is null || !Target.IsAlive;
    private bool MoveToAttackCondition()
    {
        return Target is not null && Target.IsAlive &&
            (Vector3.Distance(transform.position, Target.Position) < characterData.AttackRange || forceAttacking);
    }

}
