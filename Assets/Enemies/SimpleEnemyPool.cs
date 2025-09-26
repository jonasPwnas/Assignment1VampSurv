using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class SimpleEnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject simpleEnemyPrefab;

    [HideInInspector] public List<SimpleEnemyController> m_enemyPool = new List<SimpleEnemyController>();
    [HideInInspector] public SimpleEnemyController m_currentEnemy;
    
    public virtual void SpawnEnemy(Vector3 spawnPos)
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

    public virtual void ReturnToPool(SimpleEnemyController enemy)
    {
        m_enemyPool.Add(enemy);
    }
}
