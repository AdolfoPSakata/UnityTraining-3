using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private int waveInterval;

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
                    break;

                case GameState.NewWave:
                    yield return new WaitForSeconds(waveInterval);
                    StartWave();
                    break;

                case GameState.Restart:
                    enemyManager.DisableEnemies();
                    playerManager.RestartPlayer();
                    yield return new WaitForSeconds(waveInterval);
                    StartWave();
                    break;

                case GameState.End:
                    EventBus.openEndMenuEvent.Publish(new EventArgs(true));
                    yield return new WaitUntil(() => inputManager.GetEventSystemState());
                    break;
            }
        }
    }
    private void RestartGame(EventArgs args)
    {
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
            state = GameState.NewWave;
            return true;
        }
       
        if (!playerManager.isPlayerAlive)
        {
            state = GameState.End;
            return true;
        }
        return false;
    }
}
