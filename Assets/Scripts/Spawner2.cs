using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2 : MonoBehaviour
{
    public GameObject[] enemy;

    public float enemyOneRespawnTime = 0.8f;
    public float secondPhaseRespawnTime = 2f;
    public int firstPhaseSpawnCount = 4;
    public int secondPhaseSpawnCount = 1;
    public int randomEnemySpawnCount = 12;
    public GameController gameController;

    private bool lastEnemySpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawner());
    }

    // Update is called once per frame
    void Update()
    {
        if (lastEnemySpawned && (FindAnyObjectByType<EnemyScript>() == null && FindAnyObjectByType<BossYellow>() == null))
        {
            StartCoroutine(gameController.LevelComplete());
        }
    }

    IEnumerator EnemySpawner()
    {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(SpawnManyEnemiesTwoRandomPos(-2, -2, 0, firstPhaseSpawnCount, enemyOneRespawnTime));
        StartCoroutine(SpawnManyEnemiesTwoRandomPos(2, 2, 0, firstPhaseSpawnCount, enemyOneRespawnTime));
        StartCoroutine(SpawnEnemyOnePos(0, 1, secondPhaseSpawnCount, secondPhaseRespawnTime));

        yield return new WaitForSeconds(4f);
        StartCoroutine(SpawnEnemyOnePos(-2, 2, secondPhaseSpawnCount, secondPhaseRespawnTime));
        StartCoroutine(SpawnEnemyOnePos(2, 2, secondPhaseSpawnCount, secondPhaseRespawnTime));        

        yield return new WaitForSeconds(firstPhaseSpawnCount * enemyOneRespawnTime * 2);
        for (int i = 0; i < randomEnemySpawnCount; i++)
        {
            yield return new WaitForSeconds(0.4f);
            RandomSpawnEnemy(-3, 3, 3, 4);
        }

        yield return new WaitForSeconds(0.05f);
        StartCoroutine(SpawnManyEnemiesTwoRandomPos(-2, -2, 0, firstPhaseSpawnCount, enemyOneRespawnTime));
        StartCoroutine(SpawnManyEnemiesTwoRandomPos(2, 2, 0, firstPhaseSpawnCount, enemyOneRespawnTime));
        StartCoroutine(SpawnEnemyOnePos(0, 2, secondPhaseSpawnCount, secondPhaseRespawnTime));

        //yield return new WaitForSeconds(firstPhaseSpawnCount * enemyOneRespawnTime * 2);
        if (enemy.Length > 0 && enemy[enemy.Length - 1] != null)
        {
            yield return new WaitForSeconds(5f); // Wait some time before spawning the last enemy
            //int newEnemy = enemy.Length - 1;
            //SpawnEnemy(0, enemy.Length - 1, enemy.Length); // Spawn the last enemy at the center
            StartCoroutine(SpawnEnemyOnePos(0, enemy.Length - 1, secondPhaseSpawnCount, secondPhaseRespawnTime));
            //yield return new WaitForSeconds(firstPhaseSpawnCount * enemyOneRespawnTime * 2);
        }

        yield return new WaitForSeconds(firstPhaseSpawnCount * enemyOneRespawnTime * 2); 
        lastEnemySpawned = true;
    }

    //1 Enemy for random position
    IEnumerator SpawnManyEnemiesTwoRandomPos(int minX, int maxX, int enemyIndex, int spawnCount, float respawnTime)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            int randomXPos = Random.Range(minX, maxX);
            yield return new WaitForSeconds(respawnTime);
            SpawnEnemy(randomXPos, enemyIndex, enemyIndex + 1); // Spawn the EnemyOne 
        }
    }

    //Enemy for one position
    IEnumerator SpawnEnemyOnePos(int xPos, int enemyIndex, int spawnCount, float respawnTime)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnEnemy(xPos, enemyIndex, enemyIndex + 1);
        }
    }

    void SpawnEnemy(int xPos, int minIndex, int maxIndex)
    {
        int randomValue = Random.Range(minIndex, maxIndex);
        Instantiate(enemy[randomValue], new Vector2(xPos, transform.position.y), Quaternion.identity);
    }
    void RandomSpawnEnemy(int minX, int maxX, int minIndex, int maxIndex)
    {
        int randomXPos = Random.Range(minX, maxX);
        int randomValue = Random.Range(minIndex, maxIndex);
        Instantiate(enemy[randomValue], new Vector2(randomXPos, transform.position.y), Quaternion.identity);
    }
}
