using System;
using System.Collections;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    [Tooltip("What projectile-pool will the weapon use?")]
    [SerializeField] private GameObject projectilePool; //Probably should be instantiated? To reduce setup and prefab clutter
    
    [Tooltip("Weapon base interval, how often the weapon will be used")]
    [SerializeField] private float attackFrequency = 1.2f;

    [SerializeField] private int spawnAmount = 1;
    
    private ProjectilePool m_projectilePool;
    private int m_weaponLevel = 1;

    private void OnEnable()
    {
        PlayerData.OnPlayerDeath += OnDeath;
    }

    private void OnDisable()
    {
        PlayerData.OnPlayerDeath -= OnDeath;
    }

    private void OnDeath()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        m_projectilePool = projectilePool.GetComponent<ProjectilePool>();
        StartCoroutine(WaitForNextAttack());
    }

    private void UseWeapon(Transform target)
    {
        m_projectilePool.SpawnProjectile(target, spawnAmount);
        StartCoroutine(WaitForNextAttack());
    }

    private void GetTargetEnemy()
    {
        //Blah
    }
    
    IEnumerator WaitForNextAttack()
    {
        yield return new WaitForSeconds(attackFrequency);
        UseWeapon(transform); //Maybe shouldn't send this transform hihihihihihihi
    }
    
}