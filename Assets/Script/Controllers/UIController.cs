using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Player HUD")]
    [SerializeField] private GameObject playerHUD;

    [Header("Player Stats")]
    [SerializeField] private TMP_Text b;
    [SerializeField] private TMP_Text b1;
    [SerializeField] private TMP_Text b2;
    [SerializeField] private TMP_Text b3;
    [SerializeField] private TMP_Text b4;

    [Header("Buttons - Menu")]
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button closeMenuButton;
    

    [Header("Panels - Menu")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject confirmationPanel;

    [Header("Buttons - Option")]
    [SerializeField] private Button postButton;
    [SerializeField] private Button muteButton;
    [SerializeField] private Button closeOptionButton;

    [Header("Panels - Option")]
    //[SerializeField] private GameObject optionsPanel;
    //[SerializeField] private GameObject confirmationPanel;

    [Header("Buttons - End Game")]
    [SerializeField] private Button resetButton;
    [SerializeField] private Button leaveButton;

    [Header("Panels - End Game")]
    [SerializeField] private GameObject endGamePanel;

    [Header("Event System")]
    [SerializeField] private GameObject eventSystem;

    private UnityAction closeMenu;
    private UnityAction returnToMenu;
    private UnityAction openOptions;

    private UnityAction closeOptions;
    private UnityAction resetGame;
    private UnityAction changePostProcessing;
    private UnityAction muteGame;



    private EventSubscriber enabledPlayerHUd;
    private EventSubscriber onMenuOpen;
    private EventSubscriber onEndGameOpen;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        onMenuOpen = new EventSubscriber(EventBus.openMenuEvent, ActivateMenuPanel);
        onEndGameOpen = new EventSubscriber(EventBus.openEndMenuEvent, ActivateEndGamePanel);
        enabledPlayerHUd = new EventSubscriber(EventBus.enablePlayerHUD, ActivatePlayerHUD);

        closeMenu += delegate { CloseMenu(); };
        openOptions += delegate { ChangeActiveState(optionsPanel, true); };
        closeOptions += delegate { ChangeActiveState(optionsPanel, false); };
        returnToMenu += delegate { BackToMenu(); };
        resetGame += delegate { ResetGame(); };

        //changePostProcessing += delegate { ResetGame(); };
        //muteGame += delegate { ResetGame(); };
       
        //MENU
        SetCallBack(optionsButton, openOptions);
        SetCallBack(closeMenuButton, closeMenu);
        SetCallBack(exitButton, returnToMenu);
        //OPTION
        SetCallBack(postButton, changePostProcessing);
        SetCallBack(muteButton, muteGame);
        SetCallBack(closeOptionButton, closeOptions);
        //END GAME
        SetCallBack(resetButton, resetGame);
        SetCallBack(leaveButton, returnToMenu);
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(1);
        EventBus.disableUiInputs.Publish(new EventArgs(true));
        ChangeActiveState(eventSystem, false);
        ChangeActiveState(exitButton, false);
        ChangeActiveState(playerHUD, false);
        ChangeActiveState(menuPanel, false);
    }

    private void ResetGame()
    {
        ChangeActiveState(eventSystem, false);
        ChangeActiveState(endGamePanel, false);
        EventBus.resetGameEvent.Publish(new EventArgs(true));
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

    public void ActivatePlayerHUD(EventArgs args)
    {
        ChangeActiveState(exitButton, args.State);
        ChangeActiveState(playerHUD, args.State);
    }

    public void ActivateMenuPanel(EventArgs args)
    {
        ChangeActiveState(menuPanel, args.State);
        ChangeActiveState(eventSystem, args.State);
        if(SceneManager.GetSceneByBuildIndex(1).isLoaded)
            ChangeActiveState(exitButton.gameObject, args.State);
        else
            ChangeActiveState(exitButton.gameObject, !args.State);
    }

    public void ActivateEndGamePanel(EventArgs args)
    {
        ChangeActiveState(endGamePanel, args.State);
        ChangeActiveState(eventSystem, args.State);
    }

    private void CloseMenu()
    {
        EventBus.openMenuEvent.Publish(new EventArgs(false));
    }
}
