using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class OrbitingProjectile : ProjectileBehavior
{
    private Vector3 m_startPosition;
    private Vector3 m_lastPosition;
    private Vector3 m_currentRotation;
    private Vector3 m_smoothVelocity = Vector3.zero;
    private PlayerController m_player;
    private float m_orbitingSpeed = 100f;
    private float m_rotationX = 0.1f; //Control this value with a curve over time? mmmm
    private float m_rotationY = 0.1f; //Constant value?
    private float m_rotationSpeed = 1f;
    private float m_distanceToPlayer = 0.5f; //also change over time
    private float m_smoothingTime = 0.3f;
    private float m_heightOffset = 1.3f;
    private bool m_xUp = true;
    private bool m_doRotate;
    private float m_minRotationSpeed = 100f;
    private float m_maxRotationSpeed = 400f;
    private float m_rotationAcceleration = 50f;
    private float m_maxDistanceToPlayer = 1.4f;
    private float m_minDistanceToPlayer = 0.5f;

    [ContextMenu("UpdateVisuals")]
    private void UpdateVisuals()
    {
        SetGraphics();
    }

    private void Start()
    {
        m_player = FindFirstObjectByType<PlayerController>();
    }
    
    private void OnDisable()
    {
        baseProjectilePool.ReturnToPool(this);
    }
    
    public override void LaunchProjectile(Vector3 spawnPosition, Transform target, BaseProjectilePool projectilePool, float damage)
    {
        transform.position = spawnPosition;
        baseProjectilePool = projectilePool;
        gameObject.SetActive(true);
        StartCoroutine(RotateForDuration(13f));
    }

    public override void DisableProjectile()
    {
        gameObject.SetActive(false);
    }

    private void DoRotation()
    {
        Vector3 nextRotation = new Vector3(m_rotationX, m_rotationY);
        
        m_currentRotation = Vector3.SmoothDamp(m_currentRotation, 
            nextRotation, ref m_smoothVelocity, m_smoothingTime);
        
        transform.localEulerAngles = m_currentRotation;
        
        transform.position = m_player.transform.position - 
            transform.forward * m_distanceToPlayer + transform.up * m_heightOffset;
    }
    
    IEnumerator RotateProjectile()
    {
        DoRotation();
        m_rotationY += m_rotationSpeed * Time.deltaTime;
        if(m_xUp)
        {
            m_rotationSpeed += m_rotationAcceleration * Time.deltaTime;
            Mathf.Clamp(m_rotationSpeed, m_minRotationSpeed, m_maxRotationSpeed);
            
            m_distanceToPlayer += Time.deltaTime;
            Mathf.Clamp(m_distanceToPlayer, m_minDistanceToPlayer, m_maxDistanceToPlayer);
        }
        else
        {
            m_rotationSpeed -= m_rotationAcceleration * Time.deltaTime;
            Mathf.Clamp(m_rotationSpeed, m_minRotationSpeed, m_maxRotationSpeed);
            
            m_distanceToPlayer -= Time.deltaTime;
            Mathf.Clamp(m_distanceToPlayer, m_minDistanceToPlayer, m_maxDistanceToPlayer);
        }
        yield return new WaitForEndOfFrame();
        if(m_doRotate)
            StartCoroutine(RotateProjectile());
    }
    
    IEnumerator RotateForDuration(float duration)
    {
        m_doRotate = true;
        m_xUp = true;
        StartCoroutine(RotateProjectile());
        yield return new WaitForSeconds(duration/2);

        m_xUp = false;
        yield return new WaitForSeconds(duration/2);
        m_doRotate = false;
        StopCoroutine(RotateProjectile());
        gameObject.SetActive(false);
    }

}