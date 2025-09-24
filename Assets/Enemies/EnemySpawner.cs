using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject BatEnemy;
    
    private void Start()
    {
        StartCoroutine(EnemySpawnWait(2f));
    }

    private void SpawnEnemy()
    {
        float distance = Random.Range(7f, 25f);
        float angle = Random.Range(0f, 360f);
        Vector3 spawnPos = transform.position + Quaternion.AngleAxis(Random.Range(90f, 270f), Vector3.up) * transform.forward * distance;
        spawnPos.y = -1.7f; //should probably check this or something? IF I use something other than a plane....
        Instantiate(BatEnemy, spawnPos, Quaternion.identity);
        StartCoroutine(EnemySpawnWait(0.3f));
    }
    
    IEnumerator EnemySpawnWait(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnEnemy();
    }
}
