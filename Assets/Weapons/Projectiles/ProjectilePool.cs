using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectilePool : MonoBehaviour
{
    [Tooltip("What projectile to fire?")]
    [SerializeField] private GameObject projectilePrefab;
    
    [HideInInspector] public ProjectileData projectileData;
    
    private List<ProjectileBehavior> m_avalibleProjectiles;
    private ProjectileSpawnPositions m_spawnPositions;
    private Vector3 m_spawnPosition;
    private Vector3 m_spawnOffset;


    private void Awake()
    {
        projectileData = projectilePrefab.GetComponent<ProjectileBehavior>().projectileData;
        m_avalibleProjectiles = new List<ProjectileBehavior>(12);
        m_spawnPositions = FindFirstObjectByType<ProjectileSpawnPositions>();
    }

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
                    m_spawnOffset = Vector3.right * 1f;
                    break;
                case 2:
                    m_spawnOffset = -Vector3.right * 1f;
                    break;
            }
            
            if (m_avalibleProjectiles.Count > 0)
            {
                ProjectileBehavior avalibleProjectile = m_avalibleProjectiles[0];
                StartCoroutine(LaunchAfterTime(Random.Range(0.2f, 0.6f), avalibleProjectile, m_spawnOffset, target));
                m_avalibleProjectiles.Remove(avalibleProjectile);
            }
            else
            {
                ProjectileBehavior newProjectile = Instantiate(projectilePrefab, GetProjectilePosition(m_spawnOffset), 
                    Quaternion.identity).GetComponent<ProjectileBehavior>();
                StartCoroutine(LaunchAfterTime(Random.Range(0.2f, 0.6f), newProjectile, m_spawnOffset, target));
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
        switch (projectileData.spawnLocation)
        {
            case ProjectileData.SpawnLocation.OnTheGround:
                spawnPosition = m_spawnPositions.groundedSpawn.position;
                break;
            case ProjectileData.SpawnLocation.MiddleOfPlayer:
                spawnPosition = m_spawnPositions.middlePlayerSpawn.position;
                break;
            case ProjectileData.SpawnLocation.AbovePlayer:
                spawnPosition = m_spawnPositions.abovePlayerSpawn.position;
                break;
            case ProjectileData.SpawnLocation.SkyHigh:
                spawnPosition = m_spawnPositions.skyHighSpawn.position;
                break;
        }
        return spawnPosition + spawnOffset;
    }

    IEnumerator LaunchAfterTime(float time, ProjectileBehavior projectile, Vector3 spawnOffset, Transform target)
    {
        yield return new WaitForSeconds(time);
        projectile.LaunchProjectile(GetProjectilePosition(spawnOffset), target, this);
    }
}