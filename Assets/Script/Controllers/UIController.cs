using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour //, IEventSubscriber
{
    [Header("Player Stats")]
    [SerializeField] private TMP_Text b;
    [SerializeField] private TMP_Text b1;
    [SerializeField] private TMP_Text b2;
    [SerializeField] private TMP_Text b3;
    [SerializeField] private TMP_Text b4;

    [Header("Buttons - Menu")]
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button closeButton;

    [Header("Panels - Menu")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject confirmationPanel;

    [Header("Buttons - End Game")]
    [SerializeField] private Button resetButton;
    [SerializeField] private Button leaveButton;

    [Header("Panels - End Game")]
    [SerializeField] private GameObject endGamePanel;

    [Header("Event System")]
    [SerializeField] private GameObject eventSystem;
    private UnityAction openOptions;
    private UnityAction closeOptions;
    private UnityAction returnToMenu;
    private UnityAction resetGame;

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

        openOptions += delegate { ChangeActiveState(optionsPanel, true); };
        closeOptions += delegate { ChangeActiveState(optionsPanel, false); };

        returnToMenu += delegate { BackToMenu(); };
        resetGame += delegate { ResetGame(); };

        SetCallBack(optionsButton, openOptions);
        SetCallBack(closeButton, closeOptions);
        SetCallBack(exitButton, returnToMenu);

        SetCallBack(resetButton, resetGame);
        SetCallBack(leaveButton, returnToMenu);
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene(0);
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

    public void ActivateMenuPanel(EventArgs args)
    {
        ChangeActiveState(menuPanel, args.State);
        ChangeActiveState(eventSystem, args.State);
    }

    public void ActivateEndGamePanel(EventArgs args)
    {
        ChangeActiveState(endGamePanel, args.State);
        ChangeActiveState(eventSystem, args.State);
    }

}
