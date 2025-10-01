using UnityEngine;

namespace EquipmentLevelUp
{
    public class EquipmentLevelUpStructs : MonoBehaviour
    {
        [System.Serializable] public struct ItemLevelUpData
        {
            public string levelUpDescription;
            public float effectIncrease;
        }

        [System.Serializable] public struct ProjectileWeaponLevelUpData
        {
            public string levelUpDescription;
            public float cooldownDecrease;
            public float damageIncrease;
            public int projectileIncrease;
        }

        [System.Serializable] public struct MeleeWeaponLevelUpData
        {
            public string levelUpDescription;
            public float damageIncrease;
            public float rangeIncrease;
        }
        
        [System.Serializable] public struct AoeWeaponLevelUpData
        {
            public string levelUpDescription;
            public float cooldownDecrease;
            public float damageIncrease;
            public float rangeIncrease;
        }
    }
}
