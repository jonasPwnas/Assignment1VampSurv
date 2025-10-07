using System.Collections.Generic;
using EquipmentLevelUp;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileWeaponLevelUpData1", menuName = "Scriptable Objects/Projectile Weapon Level Up Data")]
public class ProjectileWeaponLevelUp : EquipmentLevelUpData
{
    [SerializeField] List<EquipmentLevelUpStructs.ProjectileWeaponLevelUpData> WeaponLevels;
    
    public override object GetLevelUpData(int level)
    {
        return WeaponLevels[level];
    }
    
    public override int GetMaxLevel()
    {
        return WeaponLevels.Count;
    }
}
