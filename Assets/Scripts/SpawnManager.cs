using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    private int spawnCount = 0;
    private Vector3 positionEnemySpawn = new Vector3(18f, 0.5f, 17.8f);
    void Start()
    {
        
    }

    void Update()
    {
        if(spawnCount < 3)
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
