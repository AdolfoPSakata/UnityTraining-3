using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputMap inputMap;

    public void Init(GameObject player)
    {
        inputMap = new InputMap();

        inputMap.Gameloop.Fire.performed += context => player.GetComponent<Shoter>().onShot.Invoke();

        inputMap.Gameloop.Move.started += context => player.GetComponent<Mover>().onMove.Invoke(inputMap.Gameloop.Move.ReadValue<float>());
        inputMap.Gameloop.Move.canceled += context => player.GetComponent<Mover>().onPressButton.Invoke(false);

        inputMap.Gameloop.Rotate.started += context => player.GetComponent<PlayerRotator>().onRotate.Invoke(inputMap.Gameloop.Rotate.ReadValue<float>());
        inputMap.Gameloop.Rotate.canceled += context => player.GetComponent<PlayerRotator>().onPressButton.Invoke(false);

        inputMap.Gameloop.Fire.Enable();
        inputMap.Gameloop.Move.Enable();
        inputMap.Gameloop.Rotate.Enable();
    }
}
