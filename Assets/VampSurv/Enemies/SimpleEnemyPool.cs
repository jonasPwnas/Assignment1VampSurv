using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class SimpleEnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject simpleEnemyPrefab;
    [SerializeField] private int maxSpawnAmount = 100;
    private int m_currentAmountSpawned;
    
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
            if (m_currentAmountSpawned == maxSpawnAmount)
            {
                print("MAX SPAWN REACHED IN POOL: " + name);
                return;
            }
            
            m_currentEnemy = Instantiate(simpleEnemyPrefab, spawnPos, Quaternion.identity).GetComponent<SimpleEnemyController>();
            m_currentEnemy.OnSpawn(this, spawnPos);
            m_currentAmountSpawned++;
            if(m_currentAmountSpawned > 35 && m_currentAmountSpawned < 41)
            {
                m_currentEnemy.SetAsLevelTwo();
            }
        }
    }

    public virtual void ReturnToPool(SimpleEnemyController enemy)
    {
        m_enemyPool.Add(enemy);
    }
}
