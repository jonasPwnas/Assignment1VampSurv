using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static EquipmentLevelUp.EquipmentLevelUpStructs;

public class ProjectileWeapon : EquipmentBase
{
    [Tooltip("What projectile-pool will the weapon use?")]
    [SerializeField] private GameObject projectilePool; //Probably should be instantiated? To reduce setup and prefab clutter
    
    [Tooltip("Weapon base interval, how often the weapon will be used")]
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private int spawnAmount = 1;
    [SerializeField] private int WeaponDebugLevel;
    
    private ProjectilePool m_projectilePool;
    private int m_maxColliders = 20;
    private Collider[] colliders;
    private bool m_isTargeting;
    private float m_targetLocationOffset = 10f;
    private float m_targetLocationRadius = 20f;
    private float m_damageMultiplier = 1f;
    private List<Transform> m_targets = new List<Transform>();
    
    [ContextMenu("DebugLevelEquipmentUp")]
    private void DebugLevelEquipmentUp()
    {
        WeaponDebugLevel++;
        LevelEquipmentUp(WeaponDebugLevel);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        BroccoliAtkBoost.OnBroccoliAtkBoostPowerUp += SetDamageMultiplier;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        BroccoliAtkBoost.OnBroccoliAtkBoostPowerUp -= SetDamageMultiplier;
    }

    private void SetDamageMultiplier(float damageMultiplier)
    {
        m_damageMultiplier = damageMultiplier;
        m_projectilePool.MultiplyProjectileDamage(m_damageMultiplier);
    }

    public override void LevelEquipmentUp(int level)
    {
        SetLevel(level);
        ProjectileWeaponLevelUpData data = (ProjectileWeaponLevelUpData)levelUpData.GetLevelUpData(level);
        attackCooldown *= data.cooldownDecrease;
        spawnAmount += data.projectileIncrease;
        m_projectilePool.AddProjectileDamage(data.damageIncrease);
    }

    public override string GetLevelUpDescription(int level)
    {
        ProjectileWeaponLevelUpData data = (ProjectileWeaponLevelUpData)levelUpData.GetLevelUpData(level);
        return data.levelUpDescription;
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
        SetIsEquipped(true);
        UseWeapon();
    }
    
    private void UseWeapon()
    {
        Transform target = null;
        m_targets.Clear();
        
        for (int i = 0; i < spawnAmount; i++)
        {
            if (m_isTargeting)
                target = GetTargetEnemy();
            
            float xOffset = 0;
            xOffset = i;
            int rem = i % 2;
            
            if (rem == 0)
            {
                xOffset *= -1;
            }
            
            m_projectilePool.SpawnProjectile(target, xOffset);
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
        yield return new WaitForSeconds(attackCooldown);
        UseWeapon();
    }
}