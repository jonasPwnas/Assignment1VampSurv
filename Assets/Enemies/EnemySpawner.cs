using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SimpleEnemyPool batPool;
    [SerializeField] private SimpleEnemyPool slimePool;
    [SerializeField] private SimpleEnemyPool ghostPool;
    [SerializeField] private float batFrequency = 0.8f;
    [SerializeField] private float ghostFrequency = 2f;
    [SerializeField] private float slimeFrequency = 3f;
    
    private void Start()
    {
        StartCoroutine(BatSpawnWait(2f));
        PlayerData.OnTimeThresholdReached += UpdateSpawnSettings;
    }

    private void SpawnBatEnemy()
    {
        float distance = Random.Range(7f, 25f);
        Vector3 spawnPos = transform.position + Quaternion.AngleAxis(Random.Range(90f, 270f), Vector3.up) * transform.forward * distance;
        spawnPos.y = -1.7f; //should probably check this or something? IF I use something other than a plane..
        batPool.SpawnEnemy(spawnPos);
        StartCoroutine(BatSpawnWait(batFrequency));
    }
    private void SpawnSlimeEnemy()
    {
        float distance = Random.Range(7f, 25f);
        Vector3 spawnPos = transform.position + Quaternion.AngleAxis(Random.Range(90f, 270f), Vector3.up) * transform.forward * distance;
        spawnPos.y = -1.7f; //should probably check this or something? IF I use something other than a plane..
        slimePool.SpawnEnemy(spawnPos);
        StartCoroutine(SlimeSpawnWait(slimeFrequency));
    }
    private void SpawnGhostEnemy()
    {
        float distance = Random.Range(7f, 25f);
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
}