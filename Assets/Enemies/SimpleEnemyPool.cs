using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class SimpleEnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject simpleEnemyPrefab;

    private List<SimpleEnemyController> m_enemyPool = new List<SimpleEnemyController>();
    private SimpleEnemyController m_currentEnemy;

    public void SpawnEnemy(Vector3 spawnPos)
    {
        if (m_enemyPool.Count > 0)
        {
            m_currentEnemy = m_enemyPool[0];
            m_currentEnemy.OnRespawn(spawnPos);
            m_enemyPool.Remove(m_currentEnemy);
        }
        else
        {
            m_currentEnemy = Instantiate(simpleEnemyPrefab, spawnPos, Quaternion.identity).GetComponent<SimpleEnemyController>();
            m_currentEnemy.OnSpawn(this, spawnPos);
        }
    }

    public void ReturnToPool(SimpleEnemyController enemy)
    {
        m_enemyPool.Add(enemy);
    }
}
