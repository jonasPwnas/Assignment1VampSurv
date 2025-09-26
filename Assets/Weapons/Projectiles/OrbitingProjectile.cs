using UnityEngine;

public class OrbitingProjectile : ProjectileBehavior
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
    
    public override void LaunchProjectile(Vector3 spawnPosition, Transform target, ProjectilePool pool)
    {
        projectilePool = pool;
    }

    public override void DisableProjectile()
    {
        gameObject.SetActive(false);
    }

    private void OrbitProjectile()
    {
        
    }
}