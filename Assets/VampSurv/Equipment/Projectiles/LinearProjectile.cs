using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LinearProjectile : ProjectileBehavior
{
    
    [Tooltip("Find object in VampSurv/Utility/Prefabs")]
    [SerializeField] private GameObject genericParticlePool;
    
    private Vector3 m_startPosition;
    private Vector3 m_lastPosition;
    private ParticleSystemPool m_projectilePool;
    private float m_damage = 2f;

    [ContextMenu("UpdateVisuals")]
    private void UpdateVisuals()
    {
        SetGraphics();
    }

    private void OnDisable()
    {
        projectilePool.ReturnToPool(this);
    }
    
    public override void DisableProjectile()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        SetGraphics();
        m_projectilePool = Instantiate(genericParticlePool, Vector3.zero, Quaternion.identity).GetComponent<ParticleSystemPool>();
        m_projectilePool.SetParticleSystemPrefab(projectileData.projectileImpactSystem);
    }
    
    public override void LaunchProjectile(Vector3 spawnPosition, Transform target, ProjectilePool pool, float damage)
    {
        renderer.enabled = true;
        transform.localScale = new Vector3(projectileData.projectileUniformScale, projectileData.projectileUniformScale,
            projectileData.projectileUniformScale);
        transform.position = spawnPosition;
        m_startPosition = spawnPosition;
        m_lastPosition = spawnPosition;
        projectilePool = pool;
        transform.rotation = projectilePool.transform.rotation;
        gameObject.SetActive(true);
        projectileTarget = target;
        StartCoroutine(WaitThenFire());
    }
    
    private void TargetedMove()
    {
        Vector3 refVelocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, projectileTarget.position,
            ref refVelocity, projectileData.timeToImpact);
        
        StartCoroutine(MoveProjectileToTarget());
    }

    private void UntargetedMove()
    {
        Vector3 movementDirection = Vector3.zero;
        switch (projectileData.projectileMovement)
        {
            case ProjectileData.ProjectileMovement.Forward:
                movementDirection = Vector3.forward;
                transform.Translate(movementDirection * (projectileData.linearProjectileSpeed * Time.deltaTime), Space.Self);
                break;
            case ProjectileData.ProjectileMovement.Custom:
                movementDirection = projectileData.customProjectileMovementDirection;
                transform.Translate(movementDirection * (projectileData.linearProjectileSpeed * Time.deltaTime), Space.Self);
                break;
            case ProjectileData.ProjectileMovement.Down:
                movementDirection = new Vector3(0f, -1f, 0f);
                transform.Translate(movementDirection * (projectileData.linearProjectileSpeed * Time.deltaTime), Space.Self);
                break;
            case ProjectileData.ProjectileMovement.Up:
                movementDirection = new Vector3(0f, 1f, 0f);
                transform.Translate(movementDirection * (projectileData.linearProjectileSpeed * Time.deltaTime), Space.Self);
                break;
            case ProjectileData.ProjectileMovement.DiagonalDown:
                movementDirection = new Vector3(0f, -0.5f, 1f);
                transform.Translate(movementDirection * (projectileData.linearProjectileSpeed * Time.deltaTime), Space.Self);
                break;
            case ProjectileData.ProjectileMovement.DiagonalUp:
                movementDirection = new Vector3(0f, 0.5f, 1f);
                transform.Translate(movementDirection * (projectileData.linearProjectileSpeed * Time.deltaTime), Space.Self);
                break;
        }
        meshChild.transform.rotation = Quaternion.LookRotation((transform.position - m_lastPosition).normalized);
        m_lastPosition = transform.position;
        StartCoroutine(MoveProjectileAlongDirection());
    }

    private void ProjectileImpact()
    {
        //Activate VFX plz
        //Physics.OverlapSphereNonAlloc(transform.position, projectileData.impactRadius, colliders);
        //foreach (Collider hit in colliders)
        //{
        //    hit.GetComponent<IHealthAndDamage>().TakeDamage(projectileData.projectileDamage, Vector3.up);
        //}

        m_projectilePool.SpawnSystem(transform.position);
        renderer.enabled = false;
        StartCoroutine(WaitThenDisable(1f));
    }
    
    IEnumerator WaitThenFire()
    {
        yield return new WaitForSeconds(projectileData.launchTime);
        switch (projectileData.projectileTargeting)
        {
            case ProjectileData.ProjectileTargeting.None:
                StartCoroutine(MoveProjectileAlongDirection());
                break;
            case ProjectileData.ProjectileTargeting.ClosestEnemy:
                if (projectileTarget != null)
                {
                    StartCoroutine(MoveProjectileToTarget());
                }
                else
                {
                    StartCoroutine(MoveProjectileAlongDirection());
                }
                break;
        }
    }

    IEnumerator MoveProjectileToTarget()
    {
        yield return new WaitForEndOfFrame();
        
        if (Vector3.Distance(transform.position, projectileTarget.position) <= projectileData.projectileRadius+0.1f)
        {
            ProjectileImpact();
        }
        else
        {
            TargetedMove();
        }
    }
    
    IEnumerator MoveProjectileAlongDirection()
    {   
        yield return new WaitForEndOfFrame();
        if (Vector3.Distance(m_startPosition, transform.position) > projectileData.linearProjectileTravelDistance)
        {
            ProjectileImpact();
        }
        else
        {
            UntargetedMove();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IHealthAndDamage>()?.TakeDamage(m_damage, transform.forward);
    }
    
    IEnumerator WaitThenDisable(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        DisableProjectile();
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}