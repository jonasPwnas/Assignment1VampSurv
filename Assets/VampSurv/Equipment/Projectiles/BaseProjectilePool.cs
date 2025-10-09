using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectilePool : MonoBehaviour
{
    
    [Tooltip("What projectile to fire?")]
    [SerializeField] public GameObject projectilePrefab;
    [HideInInspector] public ProjectileData projectileData;
    
    [HideInInspector] public List<ProjectileBehavior> m_avalibleProjectiles;
    [HideInInspector] public ProjectileSpawnPositions m_spawnPositions;
    [HideInInspector] public Vector3 m_spawnPosition;
    [HideInInspector] public Vector3 m_spawnOffset;
    [HideInInspector] public float m_projectileDamage;
    
    public virtual void Awake()
    {
        
    }

    public virtual void Start()
    {
        
    }

    public virtual void AddProjectileDamage(float damageToAdd)
    {
        
    }

    public virtual void MultiplyProjectileDamage(float damageMultiplier)
    {
        
    }

    public virtual void SpawnProjectile(Transform target, float xOffset)
    {
        
    }

    public virtual void ReturnToPool(ProjectileBehavior projectile)
    {
        
    }
}
