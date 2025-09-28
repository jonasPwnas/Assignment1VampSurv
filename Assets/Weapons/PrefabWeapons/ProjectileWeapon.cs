using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : WeaponBase
{
    [Tooltip("What projectile-pool will the weapon use?")]
    [SerializeField] private GameObject projectilePool; //Probably should be instantiated? To reduce setup and prefab clutter
    
    [Tooltip("Weapon base interval, how often the weapon will be used")]
    [SerializeField] private float attackFrequency = 1.2f;

    [SerializeField] private int spawnAmount = 1;
    
    private ProjectilePool m_projectilePool;
    private int m_maxColliders = 20;
    private Collider[] colliders;
    private bool m_isTargeting;
    
    public override void StartWeapon()
    {
        UseWeapon();
    }

    public override void LevelWeaponUp()
    {
        base.LevelWeaponUp();
    }

    private void Start()
    {
        m_projectilePool = projectilePool.GetComponent<ProjectilePool>();
        colliders = new Collider[m_maxColliders];

        switch (m_projectilePool.projectileData.projectileTargeting)
        {
            case ProjectileData.ProjectileTargeting.None:
                m_isTargeting = false;
                break;
            case ProjectileData.ProjectileTargeting.ClosestEnemy:
                m_isTargeting = true;
                break;
        }
        Debug.Log("DEBUG STARTING WEAPONS!!!! BE AWARE");
        StartWeapon();
    }
    

    private void UseWeapon()
    {
        Transform target = null;
        
        if (m_isTargeting)
            target = GetTargetEnemy();
        
        m_projectilePool.SpawnProjectile(target, spawnAmount);
        StartCoroutine(WaitForNextAttack());
    }

    private Transform GetTargetEnemy()
    {
        float furthestDistance = 0;
        Transform targetEnemy = null;
        int numColliders = Physics.OverlapSphereNonAlloc( transform.position + transform.forward * 3f, 10, colliders, 
            m_projectilePool.projectileData.projectileHitLayerMask);
        
        for(int i = 0; i < numColliders; i++)
        {
            Vector3 dirToTarget = colliders[i].transform.position - transform.position;
            float distanceToTargetSqr = dirToTarget.magnitude;
            if (distanceToTargetSqr > furthestDistance)
            {
                furthestDistance = distanceToTargetSqr;
                targetEnemy = colliders[i].transform;
            }
        }
        
        return targetEnemy;
    }
    
    IEnumerator WaitForNextAttack()
    {
        yield return new WaitForSeconds(attackFrequency);
        UseWeapon();
    }
}