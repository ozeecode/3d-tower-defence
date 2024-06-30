public interface IUpgradeable
{
    public int Damage { get; }
    public int Level { get; }
    public float AttackDelay { get; }
    public void Upgrade();
    public bool IsUpgradeable();
}
