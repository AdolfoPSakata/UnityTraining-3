using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private InputManager inputManager;

    private  Coroutine gameloop;
    private GameState state;
    public enum GameState
    {
        Begin,
        Waiting,
        NewWave,
        Restart,
        End,
    }

    void Start()
    {
        state = GameState.Begin;
        gameloop = StartCoroutine(StateMachineCoroutine());
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        EventSubscriber onRestartEvent = new EventSubscriber(EventBus.resetGameEvent, RestartGame);
    }

    private IEnumerator StateMachineCoroutine()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            switch (state)
            {
                case GameState.Begin:
                    Init();
                    break;

                case GameState.Waiting:
                    yield return new WaitUntil(() => NextState());
                    Debug.Log("End Waiting");
                    break;

                case GameState.NewWave:
                    yield return new WaitForSeconds(3);
                    Debug.Log("New Wave Began");
                    StartWave();
                    break;

                case GameState.Restart:
                    Debug.Log("Reset4");
                    enemyManager.DisableEnemies();
                    Debug.Log("Reset3");
                    playerManager.RestartPlayer();
                    Debug.Log("Reset2");
                    Debug.Log("Reset1");
                    StartWave();
                    Debug.Log("Reset0");
                    break;

                case GameState.End:
                    EventBus.openEndMenuEvent.Publish(new EventArgs(true));
                    yield return new WaitUntil(() => inputManager.GetEventSystemState());
                    break;
            }
            Debug.Log(state);
        }
    }

    private void RestartGame(EventArgs args)
    {
        inputManager.SwitchEventSystem();
        state = GameState.Restart;
    }

     private void StartWave()
    {
        enemyManager.StartWave();
        state = GameState.Waiting;
    }

    private void Init()
    {
        playerManager.Init();
        enemyManager.Init();
        inputManager.Init(playerManager.Player);
        state = GameState.NewWave;
    }

    private bool NextState()
    {
        if (enemyManager.CheckLiveEnemies())
        {
            Debug.Log("Enemies killed");
            state = GameState.NewWave;
            return true;
        }
       
        if (!playerManager.isPlayerAlive)
        {
            Debug.Log("Player Killed");
            inputManager.SwitchEventSystem();
            state = GameState.End;
            return true;
        }
        return false;
    }
}
