using System;
using System.Collections;
using UnityEngine;

public class LinearProjectile : ProjectileBehavior
{
    private Vector3 endPosition;
    
    private void OnDisable()
    {
        projectilePool.ReturnToPool(this);
    }

    public override void LaunchProjectile(Vector3 spawnPosition, Transform target, ProjectilePool pool)
    {
        transform.position = spawnPosition;
        endPosition = spawnPosition;
        endPosition.z += projectileData.linearProjectileTravelDistance; 
        projectilePool = pool;
        gameObject.SetActive(true);
        projectileTarget = target;
        StartCoroutine(WaitThenFire());
        print("Initiated LAUNCH!!");
    }

    public override void DisableProjectile()
    {
        gameObject.SetActive(false);
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
        switch (projectileData.projectileMovement)
        {
            case ProjectileData.ProjectileMovement.Downward:
                transform.Translate(new Vector3(0, -0.5f, 1f) * (projectileData.linearProjectileSpeed * Time.deltaTime));
                break;
            case ProjectileData.ProjectileMovement.Forward:
                transform.Translate(Vector3.forward * (projectileData.linearProjectileSpeed * Time.deltaTime));
                break;
        }
        transform.rotation = Quaternion.LookRotation(endPosition - transform.position);
        
        StartCoroutine(MoveProjectileForward());
    }

    private void ProjectileImpact()
    {
        //Activate VFX plz
        
        Physics.OverlapSphereNonAlloc(transform.position, projectileData.impactRadius, colliders);
        foreach (Collider hit in colliders)
        {
            hit.GetComponent<IHealthAndDamage>().TakeDamage(projectileData.projectileDamage, Vector3.up);
        }
        DisableProjectile(); //Should do this when particles has finished playing
    }
    
    IEnumerator WaitThenFire()
    {
        yield return new WaitForSeconds(projectileData.launchTime);
        switch (projectileData.projectileTargeting)
        {
            case ProjectileData.ProjectileTargeting.None:
                StartCoroutine(MoveProjectileForward());
                break;
            case ProjectileData.ProjectileTargeting.ClosestEnemy:
                StartCoroutine(MoveProjectileToTarget());
                break;
        }
    }

    IEnumerator MoveProjectileToTarget()
    {
        yield return new WaitForEndOfFrame();
        
        if (Vector3.Distance(transform.position, projectileTarget.position) >= projectileData.projectileRadius)
        {
            ProjectileImpact();
        }
        else
        {
            TargetedMove();
        }
    }
    
    IEnumerator MoveProjectileForward()
    {   
        yield return new WaitForEndOfFrame();
        if (Vector3.Distance(transform.position, endPosition) <= 3f)
        {
            ProjectileImpact();
        }
        else
        {
            UntargetedMove();
        }
    }
}