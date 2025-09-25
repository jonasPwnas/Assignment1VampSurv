using System;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnLocation{OnTheGround, MiddleOfPlayer, AbovePlayer, SkyHigh}

public class ProjectilePool : MonoBehaviour
{
    [Tooltip("What projectile to fire?")]
    [SerializeField] private GameObject projectilePrefab;
    
    [Tooltip("Where is the projectile spawned?")]
    [SerializeField] private SpawnLocation m_spawnLocation = SpawnLocation.AbovePlayer;
    
    private List<ProjectileBehavior> m_avalibleProjectiles;
    private ProjectileSpawnPositions m_spawnPositions;
    private Vector3 m_spawnPosition;
    private Vector3 m_spawnOffset;
    
    private void Start()
    {
        m_spawnPositions = FindFirstObjectByType<ProjectileSpawnPositions>();
        m_avalibleProjectiles = new List<ProjectileBehavior>(12);
    }

    public void SpawnProjectile(Transform target, int spawnAmount)
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            switch (i)
            {
                case 0:
                    m_spawnOffset = Vector3.zero;
                    break;
                case 1:
                    m_spawnOffset = Vector3.right * 0.5f;
                    break;
                case 2:
                    m_spawnOffset = -Vector3.right * 0.5f;
                    break;
            }
            
            if (m_avalibleProjectiles.Count > 0)
            {
                ProjectileBehavior avalibleProjectile = m_avalibleProjectiles[0];
                avalibleProjectile.LaunchProjectile(GetProjectilePosition(m_spawnOffset), target, this);
                m_avalibleProjectiles.Remove(avalibleProjectile);
            }
            else
            {
                ProjectileBehavior newProjectile = Instantiate(projectilePrefab, GetProjectilePosition(m_spawnOffset), 
                    Quaternion.identity).GetComponent<ProjectileBehavior>();
                newProjectile.LaunchProjectile(GetProjectilePosition(m_spawnOffset), target, this);
            }
        }
    }
    
    public void ReturnToPool(ProjectileBehavior projectile)
    {
        m_avalibleProjectiles.Add(projectile);
        projectile.gameObject.SetActive(false);
    }
    
    private Vector3 GetProjectilePosition(Vector3 spawnOffset)
    {
        Vector3 spawnPosition = Vector3.zero;
        switch (m_spawnLocation)
        {
            case SpawnLocation.OnTheGround:
                spawnPosition = m_spawnPositions.groundedSpawn.position;
                break;
            case SpawnLocation.MiddleOfPlayer:
                spawnPosition = m_spawnPositions.middlePlayerSpawn.position;
                break;
            case SpawnLocation.AbovePlayer:
                spawnPosition = m_spawnPositions.abovePlayerSpawn.position;
                break;
            case SpawnLocation.SkyHigh:
                spawnPosition = m_spawnPositions.skyHighSpawn.position;
                break;
        }
        return spawnPosition + spawnOffset;
    }
}