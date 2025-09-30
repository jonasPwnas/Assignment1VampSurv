using System.Collections.Generic;
using EquipmentLevelUp;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemLevelUpData", menuName = "Scriptable Objects/Item Level Up Data")]
public class ItemLevelUp : EquipmentLevelUpData
{
    [SerializeField] List<EquipmentLevelUpStructs.ItemLevelUpData> EquipmentLevels;
    
    public override object GetLevelUpData(int level)
    {
        return EquipmentLevels[level];
    }
}
