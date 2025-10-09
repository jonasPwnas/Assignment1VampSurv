using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingProjectile : ProjectileBehavior
{
    private Vector3 m_startPosition;
    private Vector3 m_lastPosition;
    private Vector3 m_currentRotation;
    private Vector3 m_smoothVelocity = Vector3.zero;
    private float m_orbitingSpeed = 1f;
    private float m_rotationX = 0.5f; //Control this value with a curve over time? mmmm
    private float m_rotationY = 0.5f; //Constant value?
    private float m_distanceToPlayer = 2f; //also change over time
    private float m_smoothingTime = 0.3f;

    [ContextMenu("UpdateVisuals")]
    private void UpdateVisuals()
    {
        SetGraphics();
    }

    
    private void OnDisable()
    {
        projectilePool.ReturnToPool(this);
    }
    
    public override void LaunchProjectile(Vector3 spawnPosition, Transform target, BaseProjectilePool pool, float damage)
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

    private void DoRotation()
    {
        Vector3 nextRotation = new Vector3(m_rotationX, m_rotationY);
        
        m_currentRotation = Vector3.SmoothDamp(m_currentRotation, 
            nextRotation, ref m_smoothVelocity, m_smoothingTime);
        
        transform.localEulerAngles = m_currentRotation;
    }
    
    IEnumerator RotateProjectile()
    {
        DoRotation();
        yield return new WaitForEndOfFrame();
    }

}