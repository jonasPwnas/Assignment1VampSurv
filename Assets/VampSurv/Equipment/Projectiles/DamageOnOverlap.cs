using System;
using Unity.VisualScripting;
using UnityEngine;

public class DamageOnOverlap : MonoBehaviour
{
    private enum DamageDirection{Forward, Backward, Left, Right}
    [SerializeField] private DamageDirection m_damageDirection = DamageDirection.Forward;
    private float m_damage = 1.8f;
    private Vector3 m_damageDirectionVector;

    private void SetDamageDirectionVector()
    {
        switch (m_damageDirection)
        {
            case DamageDirection.Forward:
                m_damageDirectionVector = transform.forward;
                break;
            case DamageDirection.Backward:
                m_damageDirectionVector = -transform.forward;
                break;
            case DamageDirection.Left:
                m_damageDirectionVector = -transform.right;
                break;
            case DamageDirection.Right:
                m_damageDirectionVector = transform.right;
                break;
        }
    }

    private void Start()
    {
        SetDamageDirectionVector();
    }

    private void OnEnable()
    {
        BroccoliAtkBoost.OnBroccoliAtkBoostPowerUp += SetDamageMultiplier;
    }
    
    private void OnDisable()
    {
        BroccoliAtkBoost.OnBroccoliAtkBoostPowerUp -= SetDamageMultiplier;
    }
    
    private void SetDamageMultiplier(float multiplier)
    {
        m_damage *= multiplier;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IHealthAndDamage>()?.TakeDamage(m_damage, m_damageDirectionVector);
    }
}