using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private InputMap inputMap;
    private EventSubscriber onUIInputDisable;
    private EventSubscriber switchEventSystemStats;
    [SerializeField] private GameObject eventSystem;

    public void Init(GameObject player)
    {
        inputMap = new InputMap();

        switchEventSystemStats = new EventSubscriber(EventBus.switchEventSystemEvent, SwitchEventSystem);
        onUIInputDisable = new EventSubscriber(EventBus.DisableUIInputsEvent, ChangeUIInputState);

        inputMap.Gameloop.Fire.performed += context => player.GetComponent<PlayerBehaviour>().onShot.Invoke();

        inputMap.Gameloop.Move.started += context => player.GetComponent<PlayerBehaviour>().onMove.Invoke(inputMap.Gameloop.Move.ReadValue<float>());
        inputMap.Gameloop.Move.canceled += context => player.GetComponent<PlayerBehaviour>().onPressButtonMover.Invoke(false);

        inputMap.Gameloop.Rotate.started += context => player.GetComponent<PlayerBehaviour>().onRotate.Invoke(inputMap.Gameloop.Rotate.ReadValue<float>());
        inputMap.Gameloop.Rotate.canceled += context => player.GetComponent<PlayerBehaviour>().onPressButtonRotator.Invoke(false);

        inputMap.UI.OpenMenu.started += context => EnableInputUI();

        inputMap.UI.OpenMenu.Enable();
        inputMap.UI.Click.Enable();
        
        EnableTankInputs();
    }

    public void DisableTankInputs()
    {
        EventBus.switchEventSystemEvent.Publish(new EventArgs(false));
        inputMap.Gameloop.Fire.Disable();
        inputMap.Gameloop.Move.Disable();
        inputMap.Gameloop.Rotate.Disable();
    }

    public void EnableTankInputs()
    {
        EventBus.switchEventSystemEvent.Publish(new EventArgs(true));
        inputMap.Gameloop.Fire.Enable();
        inputMap.Gameloop.Move.Enable();
        inputMap.Gameloop.Rotate.Enable();
    }

    private void EnableInputUI()
    {
        EventBus.openMenuEvent.Publish(new EventArgs(true));
        if (eventSystem.activeSelf == true)
            DisableTankInputs();
        else
            EnableTankInputs();
    }

    public void SwitchEventSystem(EventArgs args)
    {
        eventSystem.SetActive(args.State);
    }

    public bool GetEventSystemState()
    {
        return eventSystem.activeSelf;
    }
    public void ChangeUIInputState(EventArgs args)
    {
        if (!args.State)
        {
            DisableTankInputs();
            inputMap.UI.Click.Enable();
            inputMap.UI.OpenMenu.Enable();
        }
        else
        {
            EnableTankInputs();
            inputMap.UI.Click.Disable();
            inputMap.UI.OpenMenu.Disable();
        }
    }
}
