using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrbitingProjectilePool : BaseProjectilePool
{
    public override void Awake()
    {
        projectileData = projectilePrefab.GetComponent<ProjectileBehavior>().projectileData;
        m_avalibleProjectiles = new List<ProjectileBehavior>(12);
        m_spawnPositions = FindFirstObjectByType<ProjectileSpawnPositions>();
    }

    public override void Start()
    {
        m_spawnPositions = FindFirstObjectByType<ProjectileSpawnPositions>();
        m_avalibleProjectiles = new List<ProjectileBehavior>(12);
    }

    public override void AddProjectileDamage(float damageToAdd)
    {
        m_projectileDamage += damageToAdd;
    }

    public override void MultiplyProjectileDamage(float damageMultiplier)
    {
        m_projectileDamage *= damageMultiplier;
    }
    
    public override void SpawnProjectile(Transform target, float offset)
    {
        
        if (m_avalibleProjectiles.Count > 0)
        {
            ProjectileBehavior avalibleProjectile = m_avalibleProjectiles[0];
            StartCoroutine(LaunchAfterTime(offset, avalibleProjectile, target, m_projectileDamage));
            m_avalibleProjectiles.Remove(avalibleProjectile);
        }
        else
        {
            ProjectileBehavior newProjectile = Instantiate(projectilePrefab, GetProjectilePosition(), 
                Quaternion.identity).GetComponent<ProjectileBehavior>();
            newProjectile.SetPool(this);
            StartCoroutine(LaunchAfterTime(offset, newProjectile, target, m_projectileDamage));
        }
    }
    
    public override void ReturnToPool(ProjectileBehavior projectile)
    {
        m_avalibleProjectiles.Add(projectile);
        projectile.gameObject.SetActive(false);
    }
    
    private Vector3 GetProjectilePosition()
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
        return spawnPosition;
    }

    IEnumerator LaunchAfterTime(float time, ProjectileBehavior projectile, Transform target, float damage)
    {
        yield return new WaitForSeconds(time);
        projectile.LaunchProjectile(GetProjectilePosition(), target, damage);
    }
}