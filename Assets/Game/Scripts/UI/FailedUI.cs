public class FailedUI : PanelUI
{
    private void OnEnable()
    {
        EventManager.Subscribe<GameEvents.OnLevelFailed>(OnLevelFailed);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe<GameEvents.OnLevelFailed>(OnLevelFailed);
    }
    private void OnLevelFailed(GameEvents.OnLevelFailed failed)
    {
        Show();
    }

    public void OnRestartButtonClick()
    {
        EventManager.Fire(new GameEvents.OnLevelRestart());
        Hide();
    }
}
