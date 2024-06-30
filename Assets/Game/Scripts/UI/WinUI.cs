public class WinUI : PanelUI
{
    private void OnEnable()
    {
        EventManager.Subscribe<GameEvents.OnLevelCompleted>(OnLevelCompleted);
    }


    private void OnLevelCompleted(GameEvents.OnLevelCompleted completed)
    {
        Show();
    }

    public void OnNextButtonClick()
    {
        EventManager.Fire(new GameEvents.OnNextLevel());
        Hide();
    }
}
