using Cysharp.Threading.Tasks;
using UnityEngine;

public class DeathState : BaseState
{
    private Rigidbody rb;
    private const float RandomDeathForce = 2f;
    public DeathState(UnitAI character, Rigidbody rb) : base(character)
    {
        this.rb = rb;
    }
    public override async void OnEnter()
    {
        base.OnEnter();
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(GetRandomForce(), ForceMode.VelocityChange);
        rb.AddTorque(GetRandomForce(), ForceMode.VelocityChange);
        await UniTask.Delay(2000);
        //rb.AddTorque()
    }
    private Vector3 GetRandomForce()
    {
        return new Vector3(Rnd(), 1f, Rnd());
    }

    private float Rnd()
    {
        return Random.Range(-RandomDeathForce, RandomDeathForce);
    }
}
