using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private InputMap inputMap;
    EventSubscriber onUIInputDisable;
    [SerializeField] private GameObject eventSystem;

    public void Init(GameObject player)
    {
        inputMap = new InputMap();
        
        onUIInputDisable = new EventSubscriber(EventBus.disableUiInputs, ChangeUIInputState);
       
        inputMap.Gameloop.Fire.performed += context => player.GetComponent<PlayerBehaviour>().onShot.Invoke();

        inputMap.Gameloop.Move.started += context => player.GetComponent<PlayerBehaviour>().onMove.Invoke(inputMap.Gameloop.Move.ReadValue<float>());
        inputMap.Gameloop.Move.canceled += context => player.GetComponent<PlayerBehaviour>().onPressButtonMover.Invoke(false);

        inputMap.Gameloop.Rotate.started += context => player.GetComponent<PlayerBehaviour>().onRotate.Invoke(inputMap.Gameloop.Rotate.ReadValue<float>());
        inputMap.Gameloop.Rotate.canceled += context => player.GetComponent<PlayerBehaviour>().onPressButtonRotator.Invoke(false);

        inputMap.UI.OpenMenu.started += context => EnableInputUI();

        EnableTankInputs();

        inputMap.UI.Click.Enable();
        inputMap.UI.OpenMenu.Enable();
    }

    public void DisableTankInputs()
    {
        inputMap.Gameloop.Fire.Disable();
        inputMap.Gameloop.Move.Disable();
        inputMap.Gameloop.Rotate.Disable();
    }

    public void EnableTankInputs()
    {
        inputMap.Gameloop.Fire.Enable();
        inputMap.Gameloop.Move.Enable();
        inputMap.Gameloop.Rotate.Enable();
    }

    private void EnableInputUI()
    { 
        EventBus.openMenuEvent.Publish(new EventArgs(eventSystem.activeSelf));
        if (eventSystem.activeSelf == true)
            DisableTankInputs();
        else
            EnableTankInputs();

        SwitchEventSystem();
    }
    public void SwitchEventSystem()
    {
        eventSystem.SetActive(!eventSystem.activeSelf);
    }

    public bool GetEventSystemState()
    {
        return eventSystem.activeSelf;
    }

    public void ChangeUIInputState(EventArgs args)
    {
        if (!args.State)
        {
            inputMap.UI.Click.Enable();
            inputMap.UI.OpenMenu.Enable();
        }
        else
        {
            inputMap.UI.Click.Disable();
            inputMap.UI.OpenMenu.Disable();
        }
    }
}
