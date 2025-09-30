using Unity.VisualScripting;
using UnityEngine;

public class AttackReciever : MonoBehaviour
{
    [SerializeField] private DragonSlayerWeapon dragonSlayerWeapon;
    
    private Vector3 m_attackDirection;

    public void EnableDamageField(int AttackDirection)
    {
        switch (AttackDirection)
        {
            case 0:
                dragonSlayerWeapon.EnableCollider(false);
                break;
            case 1:
                dragonSlayerWeapon.EnableCollider(false);
                break;
            case 2:
                dragonSlayerWeapon.EnableCollider(false);
                break;
            case 3:
                dragonSlayerWeapon.EnableCollider(true);
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
        dragonSlayerWeapon.DealDamage(m_attackDirection);
    }
}
