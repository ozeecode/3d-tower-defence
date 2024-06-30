using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : PanelUI
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text playText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button deleteButton;


    private void OnEnable()
    {
        EventManager.Subscribe<GameEvents.OnLoadCompleted>(OnLoadCompleted);
        playButton.onClick.AddListener(OnStartButtonClick);
        deleteButton.onClick.AddListener(OnDeleteButtonClick);

    }
    private void OnDisable()
    {
        EventManager.Unsubscribe<GameEvents.OnLoadCompleted>(OnLoadCompleted);
        playButton.onClick.RemoveListener(OnStartButtonClick);
        deleteButton.onClick.RemoveListener(OnDeleteButtonClick);
    }
    private void OnLoadCompleted(GameEvents.OnLoadCompleted load)
    {
        if (load.levelIndex == 0)
        {
            playText.text = "Start";
            deleteButton.interactable = false;
            levelText.text = string.Empty;
        }
        else
        {
            playText.text = "Play";
            deleteButton.interactable = true;
            levelText.text = string.Format("Level {0}", load.levelIndex + 1);
        }
        Show();
    }

    public void OnStartButtonClick()
    {
        EventManager.Fire(new GameEvents.OnPlayClick());
        Hide();
    }
    public void OnDeleteButtonClick()
    {
        EventManager.Fire(new GameEvents.OnDeleteProgress());
        Hide();
    }
}
