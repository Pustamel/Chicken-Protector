using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    private int spawnCount = 0;
    private Vector3 positionEnemySpawn = new Vector3(18f, 0.5f, 17.8f);
    private int MAX_SPAWN_COUNT = 4;
    void Start()
    {
        
    }

    void Update()
    {
        if(spawnCount < MAX_SPAWN_COUNT)
        {
            Instantiate(enemy, positionEnemySpawn, Quaternion.identity);
            spawnCount++;
        }
    }

    public void DeleteSpawnedEnemy()
    {
        if(spawnCount != 0)
        {
            spawnCount--;
        }
    }
}
