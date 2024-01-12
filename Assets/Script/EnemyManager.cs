using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyGeneric;
    [SerializeField] private Transform[] spawnPoints;

    public void SpawnEnemies(Transform spawnPoint)
    {
        GameObject player = Instantiate(enemyGeneric);
        player.transform.position = spawnPoint.position;
        player.transform.parent = null;

    }

    public void StartWave()
    {
        foreach (var point in spawnPoints)
        {
            SpawnEnemies(point);
        }
    }
    void Start()
    {
        StartWave();
    }

}

