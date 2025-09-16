using Unity.VisualScripting;
using UnityEngine;

public class AttackReciever : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private DamageField damageField;
    private float m_damageMovementIntensity = 2f;
    
    private Vector3 m_attackDirection;
    
    public void DoAttack(int AttackDirection)
    {
        switch (AttackDirection)
        {
            case 0:
                m_attackDirection = transform.forward * m_damageMovementIntensity;
                break;
            case 1:
                m_attackDirection = transform.right * m_damageMovementIntensity;
                break;
            case 2:
                m_attackDirection = transform.up * m_damageMovementIntensity;
                break;
        }
        damageField.DealDamage(damage, m_attackDirection);
    }
}
