using System.Collections.Generic;
using EquipmentLevelUp;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeaponLevelUpData", menuName = "Scriptable Objects/Melee Weapon Level Up Data")]
public class MeleeWeaponLevelUp : EquipmentLevelUpData
{
    [SerializeField] List<EquipmentLevelUpStructs.MeleeWeaponLevelUpData> WeaponLevels;
    
    public override object GetLevelUpData(int level)
    {
        return WeaponLevels[level];
    }
}
