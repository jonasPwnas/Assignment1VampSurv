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
    private float m_targetLocationOffset = 10f;
    private float m_targetLocationRadius = 20f;
    private List<Transform> m_targets = new List<Transform>();
    
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
        m_targets.Clear();
        
        for (int i = 0; i < spawnAmount; i++)
        {
            if (m_isTargeting)
                target = GetTargetEnemy();
            
            m_projectilePool.SpawnProjectile(target);
        }
        StartCoroutine(WaitForNextAttack());
    }

    private Transform GetTargetEnemy()
    {
        float furthestDistance = 0;
        Transform targetEnemy = null;
        int numColliders = Physics.OverlapSphereNonAlloc(GetTargetingPosition(), m_targetLocationRadius, colliders, 
            m_projectilePool.projectileData.projectileHitLayerMask);
        
        for(int i = 0; i < numColliders; i++)
        {
            Vector3 dirToTarget = colliders[i].transform.position - GetTargetingPosition();
            float distanceToTargetSqr = dirToTarget.magnitude;
            if (distanceToTargetSqr > furthestDistance)
            {
                if(m_targets.Count > 0)
                {
                    foreach (Transform t in m_targets)
                    {
                        if (t != colliders[i].transform)
                        {
                            furthestDistance = distanceToTargetSqr;
                            targetEnemy = colliders[i].transform;
                        }
                    }
                }
                else
                {
                    furthestDistance = distanceToTargetSqr;
                    targetEnemy = colliders[i].transform;
                }
            }
        }
        
        m_targets.Add(targetEnemy);
        return targetEnemy;
    }
    
    private Vector3 GetTargetingPosition()
    {
        return transform.position + transform.forward * m_targetLocationOffset;
    }
    
    IEnumerator WaitForNextAttack()
    {
        yield return new WaitForSeconds(attackFrequency);
        UseWeapon();
    }
}