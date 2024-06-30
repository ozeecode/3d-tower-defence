using KBCore.Refs;
using Lean.Pool;
using Ozee.StateMachine;
using UnityEngine;

public class UnitAI : CharacterBase
{
    protected static int BASE_COLOR = Shader.PropertyToID("_BaseColor");
    public Vector3 TargetPosition => Target is not null && Target.IsAlive ? Target.Position : characterData.TargetPosition;
    public IDamageable Target { get; protected set; }


    [SerializeField, Anywhere] private MeshRenderer meshRenderer;
    [SerializeField, Self] protected Animator animator;

    private MaterialPropertyBlock materialPropertyBlock;

    [HideInInspector] public bool forceAttacking;

    protected override void Awake()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Target = null;
        forceAttacking = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        SetColor(characterData.Color);

    }
    private void SetColor(Color color)
    {
        materialPropertyBlock.SetColor(BASE_COLOR, color);
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }


    protected override void StateMachineInit()
    {
        MoveState moveState = new MoveState(rb, this, characterData);
        AttackState attackState = new AttackState(this, animator, characterData);
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


    public void FindTarget()
    {
        if (Target is not null && Target.IsAlive)
        {
            return;
        }
        Target = Radar<IDamageable>.FindNearestTarget(transform.position, characterData.RadarRange, characterData.targetLayer);
    }

    public void SetForcedTarget(IDamageable target)
    {
        Target = target;
        forceAttacking = true;
    }

    protected override void Death()
    {
        LeanPool.Despawn(this, 3);
    }


}


public class BaseState : IState
{
    protected UnitAI character;

    public BaseState(UnitAI character)
    {
        this.character = character;
    }

    public virtual void FixedTick()
    {

    }

    public virtual void OnEnter()
    {
        character.State = GetType().Name;
    }

    public virtual void OnExit()
    {

    }

    public virtual void Tick()
    {

    }
}