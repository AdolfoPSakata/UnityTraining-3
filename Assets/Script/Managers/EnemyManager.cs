using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Tank Parts")]
    [SerializeField] private BodyConfig[] bodyConfigs;
    [SerializeField] private CannonConfig[] cannonConfigs;
    [SerializeField] private TurretConfig[] turretConfigs;
    [SerializeField] private WheelConfig[] wheelConfigs;

    [Header("Enemy prefab")]
    [SerializeField] private GameObject enemyGeneric;

    [Header("Configs")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform poolOrigin;
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private TankConfig tankConfig;

    const int MAX_ENEMY = 20;

    List<GameObject> activeEnemies = new List<GameObject>();
    Queue<GameObject> enemyQueue = new Queue<GameObject>();

    public void SpawnEnemies(Transform spawnPoint)
    {
        for (int i = 0; i < MAX_ENEMY; i++)
        {
            GameObject enemy = Instantiate(enemyGeneric);
            enemy.SetActive(false);
            enemy.transform.position = spawnPoint.position;
            enemy.transform.parent = enemiesParent;
            enemyQueue.Enqueue(enemy);
        }
    }

    public void DisableEnemies()
    {
        int count = activeEnemies.Count;

        for (int i = 0; i < count; i++)
        {
            activeEnemies[i].GetComponent<EnemyBehaviour>().StopAllCoroutines();
            RemoveFromList(activeEnemies[i]);
        }
    }

    public void RemoveFromList(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        enemy.SetActive(false);
        enemy.transform.position = poolOrigin.position;
    }

    public TankConfig RandomizeEnemy(TankConfig tankConfig)
    {
        tankConfig.body = bodyConfigs[Random.Range(0, bodyConfigs.Length)];
        tankConfig.cannon = cannonConfigs[Random.Range(0, cannonConfigs.Length)];
        tankConfig.turret = turretConfigs[Random.Range(0, turretConfigs.Length)];
        tankConfig.wheel = wheelConfigs[Random.Range(0, wheelConfigs.Length)];
        return tankConfig;
    }

    public void StartWave()
    {
        foreach (var point in spawnPoints)
        {
            GameObject enemy = enemyQueue.Dequeue();
            EnemyBehaviour enemyBehaviours = enemy.GetComponent<EnemyBehaviour>();
            enemy.transform.position = point.position;
            enemyBehaviours.ConfigureEnemy(RandomizeEnemy(tankConfig));
            enemy.SetActive(true);
            enemyBehaviours.Init();
            enemyBehaviours.onDeath = RemoveFromList;
            activeEnemies.Add(enemy);
        }
    }
    public void Init()
    {
        SpawnEnemies(poolOrigin);
    }

    public bool CheckLiveEnemies()
    {
        return activeEnemies.Count <= 0;
    }
}

