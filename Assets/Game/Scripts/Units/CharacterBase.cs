using KBCore.Refs;
using Ozee.StateMachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class CharacterBase : MonoBehaviour, IDamageable
{
    #region Properties
    public string State; //FOR DEBUG PURPOSE!
    public Transform Transform => transform;
    public Vector3 Position => transform.position;
    public bool IsAlive => currentHealth > 0;
    public int Health => currentHealth;
    public Transform DamagePoint => damagePoint;
    #endregion

    #region References
    [SerializeField, Self] protected Rigidbody rb;
    [SerializeField, Anywhere] protected Transform damagePoint;
    [SerializeField, Anywhere] protected CharacterDataSO characterData;
    #endregion


    protected int currentHealth;
    protected StateMachine stateMachine;

    protected virtual void Awake()
    {
        StateMachineInit();
    }

    protected virtual void OnEnable()
    {
        currentHealth = characterData.MaxHealth;
    }

    protected virtual void Update()
    {
        stateMachine.Tick();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.FixedTick();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }



    protected abstract void StateMachineInit();
    protected abstract void Death();
}