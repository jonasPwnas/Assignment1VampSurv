using Unity.VisualScripting;
using UnityEngine;

public class AttackReciever : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private DamageField damageField;
    
    private Vector3 m_attackDirection;

    public void EnableDamageField(int AttackDirection)
    {
        switch (AttackDirection)
        {
            case 0:
                damageField.EnableCollider(false);
                break;
            case 1:
                damageField.EnableCollider(false);
                break;
            case 2:
                damageField.EnableCollider(false);
                break;
            case 3:
                damageField.EnableCollider(true);
                break;
        }
    }
    
    public void DoAttack(int AttackDirection)
    {
        switch (AttackDirection)
        {
            case 0:
                m_attackDirection = transform.forward;
                break;
            case 1:
                m_attackDirection = transform.right + transform.up * 0.1f;
                break;
            case 2:
                m_attackDirection = transform.up * 0.18f;
                break;
            case 3:
                m_attackDirection = transform.up * 0.18f + transform.forward;
                break;
        }
        damageField.DealDamage(damage, m_attackDirection);
    }
}
