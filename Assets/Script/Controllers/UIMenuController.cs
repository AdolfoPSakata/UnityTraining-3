using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private GameObject eventSystem;
    private EventSubscriber onMenuOpen;

    private void Start()
    {
        if (!SceneManager.GetSceneByBuildIndex(2).isLoaded)
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
       
        playButton.onClick.AddListener(Play);
        exitButton.onClick.AddListener(delegate { Application.Quit(); });
        optionButton.onClick.AddListener(EnableOptionMenu);

        onMenuOpen = new EventSubscriber(EventBus.openMenuEvent, EnableEventSystem);
    }

    private void EnableOptionMenu()
    {
        EventBus.openMenuEvent.Publish(new EventArgs(eventSystem.activeSelf));
    }

    private void EnableEventSystem(EventArgs args)
    {
        eventSystem.SetActive(!args.State);
    }

    private void Play()
    {
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
}
