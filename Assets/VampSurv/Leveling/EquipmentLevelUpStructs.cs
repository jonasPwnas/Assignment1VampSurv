using UnityEngine;

namespace EquipmentLevelUp
{
    public class EquipmentLevelUpStructs : MonoBehaviour
    {
        [System.Serializable] public struct ItemLevelUpData
        {
            public float effectIncrease;
        }

        [System.Serializable] public struct ProjectileWeaponLevelUpData
        {
            public float cooldownDecrease;
            public float damageIncrease;
            public int projectileIncrease;
        }

        [System.Serializable] public struct MeleeWeaponLevelUpData
        {
            public float damageIncrease;
            public float rangeIncrease;
        }
        
        [System.Serializable] public struct AoeWeaponLevelUpData
        {
            public float cooldownDecrease;
            public float damageIncrease;
            public float rangeIncrease;
        }
    }
}
