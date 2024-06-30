using UnityEngine;

public class MoveState : BaseState
{
    private Rigidbody rb;
    private CharacterDataSO characterData;

    public MoveState(Rigidbody rb, UnitAI character, CharacterDataSO characterData) : base(character)
    {
        this.rb = rb;
        this.characterData = characterData;
    }

    public override void FixedTick()
    {
        Movement();
        character.FindTarget();
    }

    public override void OnExit()
    {
        base.OnExit();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    private Quaternion LookDirection(Vector3 dir)
    {
        if (dir == Vector3.zero) return character.transform.rotation;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, angle, 0));
    }
    private void Movement()
    {
        Vector3 dir = character.TargetPosition - character.transform.position;
        dir.y = 0;
        if (dir.magnitude > 1f)
        {
            dir.Normalize();
        }
        Quaternion rotation = LookDirection(dir);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotation, characterData.RotationSpeed));
        rb.velocity = dir * characterData.MoveSpeed;
    }
}
