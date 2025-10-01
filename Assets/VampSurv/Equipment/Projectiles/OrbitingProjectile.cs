using System.Collections;
using UnityEngine;

public class OrbitingProjectile : ProjectileBehavior
{
    private Vector3 m_startPosition;
    private Vector3 m_lastPosition;
    private Vector3 m_rotationDirection;
    private float m_orbitingSpeed = 1f;


    private void Start()
    {
        m_rotationDirection = new Vector3(0f, 1f, Random.Range(-0.3f, 0.3f));
    }
    
    [ContextMenu("UpdateVisuals")]
    private void UpdateVisuals()
    {
        SetGraphics();
    }

    private void OnDisable()
    {
        projectilePool.ReturnToPool(this);
    }
    
    public override void LaunchProjectile(Vector3 spawnPosition, Transform target, ProjectilePool pool, float damage)
    {
        projectilePool = pool;
    }

    public override void DisableProjectile()
    {
        gameObject.SetActive(false);
    }

    private void OrbitProjectile()
    {
        //transform.localEulerAngles;
    }

}