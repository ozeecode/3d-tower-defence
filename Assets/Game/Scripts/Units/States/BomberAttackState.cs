public class BomberAttackState : BaseState
{
    private CharacterDataSO characterData;
    public BomberAttackState(UnitAI character, CharacterDataSO characterData) : base(character)
    {
        this.characterData = characterData;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Explode();
    }

    private void Explode()
    {
        character.Target.TakeDamage(characterData.Damage);
        character.TakeDamage(character.Health);

    }


}