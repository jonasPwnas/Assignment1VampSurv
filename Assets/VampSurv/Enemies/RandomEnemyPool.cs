using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomEnemyPool : SimpleEnemyPool
{
    [SerializeField] private List<GameObject> simpleEnemyPrefabs;

    private int m_maxSpawnAmount = 50;
    private int m_currentSpawnAmount;

    public override void SpawnEnemy(Vector3 spawnPos)
    {
        if (m_enemyPool.Count > 0)
        {
            m_currentEnemy = m_enemyPool[0];
            m_currentEnemy.OnRespawn(spawnPos);
            m_enemyPool.Remove(m_currentEnemy);
        }
        else
        {
            if (m_currentSpawnAmount < m_maxSpawnAmount)
            {
                int randomSpawnIndex = Random.Range(0, simpleEnemyPrefabs.Count);
                m_currentEnemy = Instantiate(simpleEnemyPrefabs[randomSpawnIndex], spawnPos, Quaternion.identity).GetComponent<SimpleEnemyController>();
                m_currentEnemy.OnSpawn(this, spawnPos);
                m_currentSpawnAmount++;
            }
        }
    }
    
    public override void ReturnToPool(SimpleEnemyController enemy)
    {
        m_enemyPool.Add(enemy);
    }
}
