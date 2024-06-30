public interface ISelectable
{
    public bool IsActive { get; }
    public void Select();
    public void Deselect();
}
