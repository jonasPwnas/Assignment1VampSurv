using System.Collections.Generic;
using EquipmentLevelUp;
using UnityEngine;

[CreateAssetMenu(fileName = "AoeWeaponLevelUpData", menuName = "Scriptable Objects/Aoe Weapon Level Up Data")]
public class AoeWeaponLevelUp : EquipmentLevelUpData
{
    [SerializeField] List<EquipmentLevelUpStructs.AoeWeaponLevelUpData> WeaponLevels;
    
    public override object GetLevelUpData(int level)
    {
        return WeaponLevels[level];
    }
}
