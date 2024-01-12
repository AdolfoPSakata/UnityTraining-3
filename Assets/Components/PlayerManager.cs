using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerGeneric;
    [SerializeField] private Transform spawnPoints;
    private InputMap inputMap;
    public void SpawnPlayer(Transform spawnPoint)
    {
        GameObject player = Instantiate(playerGeneric);
        player.transform.position = spawnPoint.position;
        player.transform.parent = null;

        ///////////////////////
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
    void Start()
    {
        SpawnPlayer(spawnPoints);
    }

}
