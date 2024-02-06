using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button garageButton;

    private void Start()
    {
        if (!SceneManager.GetSceneByBuildIndex(2).isLoaded)
            SceneManager.LoadScene(2, LoadSceneMode.Additive);

        playButton.onClick.AddListener(Play);
        exitButton.onClick.AddListener(delegate { Application.Quit(); });
        optionButton.onClick.AddListener(EnableOptionMenu);
        garageButton.onClick.AddListener(EnableGarageMenu);
    }

    private void EnableOptionMenu()
    {
        EventBus.openMenuEvent.Publish(new EventArgs(true));
    }
    private void EnableGarageMenu()
    {
        EventBus.openGarageEvent.Publish(new EventArgs(true));
    }
    private void Play()
    {
        EventBus.enablePlayerHUDEvent.Publish(new EventArgs(true));
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
}
