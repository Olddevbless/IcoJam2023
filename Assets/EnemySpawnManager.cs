using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] List<GameObject> enemySpawners;
    [SerializeField] float spawnCooldown;
    float spawnTimer;
    [SerializeField] GameObject enemyPrefab;
    int spawnCounter;
    void Start()
    {
        foreach(GameObject enemySpawner in enemySpawners)
        {
            enemySpawner.transform.position = new Vector3(Random.Range(5, 50), 0, Random.Range(5, 55));
        }
        spawnCounter = 1;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <=0)
        {
            
            SpawnEnemy(spawnCounter);
          
            spawnTimer = spawnCooldown;
        }
        
    }

    void SpawnEnemy(int enemiesToSpawn)
    {
        if (FindObjectsOfType<EnemyFSM>().Length>30)
        {
            return;
        }
        else
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Instantiate(enemyPrefab, enemySpawners[Random.Range(0, enemySpawners.Count)].transform.position, Quaternion.identity);
            }
            spawnCounter++;
        }
       
    }
}
