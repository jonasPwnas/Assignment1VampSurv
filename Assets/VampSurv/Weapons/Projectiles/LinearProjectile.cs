using System;
using System.Collections;
using UnityEngine;

public class LinearProjectile : ProjectileBehavior
{
    private Vector3 startPosition;
    private Vector3 lastPosition;

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

    public override void LaunchProjectile(Vector3 spawnPosition, Transform target, ProjectilePool pool)
    {
        renderer.enabled = true;
        transform.localScale = new Vector3(projectileData.projectileUniformScale, projectileData.projectileUniformScale,
            projectileData.projectileUniformScale);
        transform.position = spawnPosition;
        startPosition = spawnPosition;
        lastPosition = spawnPosition;
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
        meshChild.transform.rotation = Quaternion.LookRotation((transform.position - lastPosition).normalized);
        lastPosition = transform.position;
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

        Instantiate(projectileData.projectileImpactSystem, transform.position, Quaternion.identity, transform);
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
        if (Vector3.Distance(startPosition, transform.position) > projectileData.linearProjectileTravelDistance)
        {
            ProjectileImpact();
        }
        else
        {
            UntargetedMove();
        }
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