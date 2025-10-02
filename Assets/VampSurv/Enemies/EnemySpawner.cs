using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SimpleEnemyPool batPool;
    [SerializeField] private SimpleEnemyPool slimePool;
    [SerializeField] private SimpleEnemyPool ghostPool;
    [SerializeField] private RandomEnemyPool randomPool;
    [SerializeField] private float batFrequency = 0.8f;
    [SerializeField] private float ghostFrequency = 2f;
    [SerializeField] private float slimeFrequency = 3f;
    [SerializeField] private float randomFrequency = 3.5f;
    
    private void OnEnable()
    {
        PlayerData.OnSessionTimeThresholdReached += UpdateSpawnSettings;
        PlayerData.OnPlayerDeath += OnDeath;
    }
    
    private void OnDisable()
    {
        PlayerData.OnSessionTimeThresholdReached -= UpdateSpawnSettings;
        PlayerData.OnPlayerDeath -= OnDeath;
    }

    private void OnDeath()
    {
        StopAllCoroutines();
    }
    
    private void Start()
    {
        StartCoroutine(BatSpawnWait(2f));
        SpawnStarterEnemies(7);
    }
    
    private float GetNearRandomSpawnRange()
    {
        return Random.Range(23f, 40f);
    }
    
    private float GetMidRandomSpawnRange()
    {
        return Random.Range(35f, 50f);
    }
    
    private float GetFarRandomSpawnRange()
    {
        return Random.Range(40f, 120f);
    }

    private void SpawnRandomFarEnemy()
    {
        float distance = GetFarRandomSpawnRange();
        Vector3 spawnPos = transform.position + Quaternion.AngleAxis(Random.Range(-90f, 90f), Vector3.up) * transform.forward * distance;
        spawnPos.y = -1.7f; //should probably check this or something? IF I use something other than a plane..
        randomPool.SpawnEnemy(spawnPos);
        StartCoroutine(RandomSpawnWait(randomFrequency));
    }
    
    private void SpawnBatEnemy()
    {
        float distance = GetMidRandomSpawnRange();
        Vector3 spawnPos = transform.position + Quaternion.AngleAxis(Random.Range(90f, 270f), Vector3.up) * transform.forward * distance;
        spawnPos.y = -1.7f; //should probably check this or something? IF I use something other than a plane..
        batPool.SpawnEnemy(spawnPos);
        StartCoroutine(BatSpawnWait(batFrequency));
    }
    private void SpawnSlimeEnemy()
    {
        float distance = GetMidRandomSpawnRange();
        Vector3 spawnPos = transform.position + Quaternion.AngleAxis(Random.Range(90f, 270f), Vector3.up) * transform.forward * distance;
        spawnPos.y = -1.7f; //should probably check this or something? IF I use something other than a plane..
        slimePool.SpawnEnemy(spawnPos);
        StartCoroutine(SlimeSpawnWait(slimeFrequency));
    }
    private void SpawnGhostEnemy()
    {
        float distance = GetMidRandomSpawnRange();
        Vector3 spawnPos = transform.position + Quaternion.AngleAxis(Random.Range(90f, 270f), Vector3.up) * transform.forward * distance;
        spawnPos.y = -1.7f; //should probably check this or something? IF I use something other than a plane..
        ghostPool.SpawnEnemy(spawnPos);
        StartCoroutine(GhostSpawnWait(ghostFrequency));
    }

    private void UpdateSpawnSettings(int timeThreshold)
    {
        switch (timeThreshold)
        {
            case 1:
                batFrequency *= 0.8f;
                StartCoroutine(GhostSpawnWait(ghostFrequency));
                StartCoroutine(RandomSpawnWait(0.1f));
                break;
            case 2:
                batFrequency *= 0.6f;
                ghostFrequency*= 0.7f;
                break;
            case 4:
                StartCoroutine(SlimeSpawnWait(slimeFrequency));
                break;
            case 6:
                batFrequency = 1.5f;
                ghostFrequency = 2f;
                slimeFrequency = 4f;
                break;
            case 8:
                batFrequency *= 0.3f;
                ghostFrequency*= 0.3f;
                slimeFrequency*= 0.3f;
                break;
        }
    }

    private void SpawnStarterEnemies(int amount)
    {
        int i;
        for (i = 0; i < amount; i++)
        {
            float distance = GetMidRandomSpawnRange();
            Vector3 spawnPos = transform.position + Quaternion.AngleAxis(Random.Range(-90f, 90f), Vector3.up) * transform.forward * distance;
            spawnPos.y = -1.7f; //should probably check this or something? IF I use something other than a plane..
            batPool.SpawnEnemy(spawnPos);
        }
    }
    
    IEnumerator BatSpawnWait(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnBatEnemy();
    }
    IEnumerator SlimeSpawnWait(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnSlimeEnemy();
    }
    IEnumerator GhostSpawnWait(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnGhostEnemy();
    }
    IEnumerator RandomSpawnWait(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnRandomFarEnemy();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}