using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SimpleEnemyPool batPool;
    [SerializeField] private float spawnFrequency = 1.1f; //Change spawn amount instead of frequency?
    
    private void Start()
    {
        StartCoroutine(EnemySpawnWait(2f));
    }

    private void SpawnEnemy()
    {
        float distance = Random.Range(7f, 25f);
        Vector3 spawnPos = transform.position + Quaternion.AngleAxis(Random.Range(90f, 270f), Vector3.up) * transform.forward * distance;
        spawnPos.y = -1.7f; //should probably check this or something? IF I use something other than a plane..
        batPool.SpawnEnemy(spawnPos);
        StartCoroutine(EnemySpawnWait(spawnFrequency));
    }
    
    IEnumerator EnemySpawnWait(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnEnemy();
    }
}
