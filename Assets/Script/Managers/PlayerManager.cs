using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] private GameObject playerGeneric;
    [SerializeField] private Transform spawnPoints;
    [SerializeField] private Transform tankParent;
    [SerializeField] private TankConfig tankConfig;
    public GameObject Player { get; private set; }
    public PlayerBehaviour PlayerBehaviour { get; private set; }
    public bool isPlayerAlive { get; private set; } = false;
    public void Init()
    {
        SpawnPlayer(spawnPoints);
    }

    private void SpawnPlayer(Transform spawnPoint)
    {
        Player = Instantiate(playerGeneric);
        PlayerBehaviour = Player.GetComponent<PlayerBehaviour>();
        PlayerBehaviour.ConfigurePlayer(tankConfig);
        PlayerBehaviour.SetupInputs();
        PlayerBehaviour.onDeath = ChangePlayerStatus;
        
        Player.transform.position = spawnPoint.position;
        Player.transform.parent = tankParent;

        ChangePlayerStatus();
    }

    public void ChangePlayerStatus()
    {
        isPlayerAlive = !isPlayerAlive;
        EventBus.enablePlayerHUDEvent.Publish(new EventArgs(isPlayerAlive));
    }

    public void RestartPlayer()
    {
        ChangePlayerStatus();
        Player.gameObject.SetActive(true);
        PlayerBehaviour.SetConfigValue();
        Player.gameObject.transform.position = spawnPoints.transform.position;
        Player.gameObject.transform.rotation = spawnPoints.transform.rotation;
        PlayerBehaviour.SetupInputs();
    }
}
