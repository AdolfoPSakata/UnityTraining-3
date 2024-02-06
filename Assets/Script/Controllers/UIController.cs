using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Player HUD")]
    [SerializeField] private GameObject playerHUD;

    [Header("Buttons - Menu")]
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button closeGarageButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button closeMenuButton;

    [Header("Panels - Menu")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private GameObject garagePanel;

    [Header("Buttons - Option")]
    [SerializeField] private Button postButton;
    [SerializeField] private Button muteButton;
    [SerializeField] private Button closeOptionButton;

    [Header("Buttons - End Game")]
    [SerializeField] private Button resetButton;
    [SerializeField] private Button leaveButton;

    [Header("Panels - End Game")]
    [SerializeField] private GameObject endGamePanel;

    [Header("Event System")]
    [SerializeField] private GameObject eventSystem;

    private UnityAction closeMenu;
    private UnityAction returnToMenu;

    private UnityAction openGarageMenu;
    private UnityAction closeGarageMenu;

    private UnityAction openOptions;
    private UnityAction closeOptions;

    private UnityAction resetGame;
    private UnityAction changePostProcessing;
    private UnityAction muteGame;

    private EventSubscriber enabledPlayerHUd;
    private EventSubscriber onMenuOpen;
    private EventSubscriber onGarageOpen;
    private EventSubscriber onEndGameOpen;
    private EventSubscriber switchEventSystemStats;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        switchEventSystemStats = new EventSubscriber(EventBus.switchEventSystemEvent, SwitchEventSystem);

        enabledPlayerHUd = new EventSubscriber(EventBus.enablePlayerHUDEvent, SetPlayerHUDState);
        onMenuOpen = new EventSubscriber(EventBus.openMenuEvent, SetMenuState);
        

        onEndGameOpen = new EventSubscriber(EventBus.openEndMenuEvent, SetEndGamePanelState);
        onGarageOpen = new EventSubscriber(EventBus.openGarageEvent, OpenGaragePanel);

        closeMenu += delegate { CloseMenu(); };
        openGarageMenu += delegate { ChangeActiveState(garagePanel, true); };
        closeGarageMenu += delegate { CloseGarage(); };

        openOptions += delegate { ChangeActiveState(optionsPanel, true); };
        closeOptions += delegate { ChangeActiveState(optionsPanel, false); };

        returnToMenu += delegate { BackToMenu(); };
        resetGame += delegate { ResetGame(); };

        //MENU
        SetCallBack(optionsButton, openOptions);
        SetCallBack(closeMenuButton, closeMenu);
        SetCallBack(exitButton, returnToMenu);
        //OPTION
        SetCallBack(postButton, changePostProcessing);
        SetCallBack(muteButton, muteGame);
        SetCallBack(closeOptionButton, closeOptions);
        //GARAGE
        SetCallBack(closeGarageButton, closeGarageMenu);
        //END GAME
        SetCallBack(resetButton, resetGame);
        SetCallBack(leaveButton, returnToMenu);
    }
    private void SetCallBack(Button button, UnityAction callback)
    {
        button.onClick.AddListener(callback);
    }
    private void ChangeActiveState(GameObject obj, bool isActive)
    {
        obj.SetActive(isActive);
    }
    private void ChangeActiveState(Button obj, bool isActive)
    {
        obj.enabled = isActive;
    }
    public void SetPlayerHUDState(EventArgs args)
    {
        ChangeActiveState(exitButton, args.State);
        ChangeActiveState(playerHUD, args.State);
    }
    private void BackToMenu()
    {
        EventBus.switchEventSystemEvent.Publish(new EventArgs(!true));
        EventBus.DisableUIInputsEvent.Publish(new EventArgs(false));
        EventBus.ResetEventSubscriber(ref EventBus.switchEventSystemEvent);
        EventBus.ResetEventSubscriber(ref EventBus.ammoChangeEvent);
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(1);

        ChangeActiveState(exitButton, false);
        ChangeActiveState(playerHUD, false);
        ChangeActiveState(menuPanel, false);
        ChangeActiveState(endGamePanel, false);

        switchEventSystemStats = new EventSubscriber(EventBus.switchEventSystemEvent, SwitchEventSystem);
    }
    private void ResetGame()
    {
        EventBus.switchEventSystemEvent.Publish(new EventArgs(!false));
        EventBus.resetGameEvent.Publish(new EventArgs(true));
        ChangeActiveState(endGamePanel, false);
    }
    public void SetEndGamePanelState(EventArgs args)
    {
        EventBus.switchEventSystemEvent.Publish(new EventArgs(!true));
        ChangeActiveState(endGamePanel, args.State);
    }
    public void SetMenuState(EventArgs args)
    {
        ChangeActiveState(menuPanel, args.State);

        if (SceneManager.GetSceneByBuildIndex(0).isLoaded)
            ChangeActiveState(exitButton.gameObject, !args.State);
        else
            ChangeActiveState(exitButton.gameObject, args.State);
    }
    private void CloseMenu()
    {
        if (!SceneManager.GetSceneByBuildIndex(0).isLoaded)
            EventBus.switchEventSystemEvent.Publish(new EventArgs(!false));
        ChangeActiveState(menuPanel, false);
    }
    public void OpenGaragePanel(EventArgs args)
    {
        ChangeActiveState(garagePanel, args.State);
    }
    private void CloseGarage()
    {
        ChangeActiveState(garagePanel, false);
    }
    public void SwitchEventSystem(EventArgs args)
    {
        eventSystem.SetActive(!args.State);
    }
}
