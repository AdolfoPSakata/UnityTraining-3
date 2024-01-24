using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyGeneric;
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

    public void StartWave()
    {
        foreach (var point in spawnPoints)
        {
            GameObject enemy = enemyQueue.Dequeue();
            enemy.transform.position = point.position;
            enemy.SetActive(true);
            enemy.GetComponent<EnemyBehaviour>().ConfigureEnemy(tankConfig);
            enemy.GetComponent<EnemyBehaviour>().Init();
            enemy.GetComponent<EnemyBehaviour>().onDeath = RemoveFromList;
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

