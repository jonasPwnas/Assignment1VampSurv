using UnityEngine;
using static EquipmentLevelUp.EquipmentLevelUpStructs;

public class AttackBoost : EquipmentBase
{
    public delegate void AttackBoostPowerUp(float multiplier);
    public static event AttackBoostPowerUp OnAttackBoostPowerUp;
    private float m_damageMultiplier = 1.1f;
    
    public override void StartEquipment()
    {
        OnAttackBoostPowerUp.Invoke(m_damageMultiplier);
    }
    
    public override string GetLevelUpDescription(int level)
    {
        ItemLevelUpData itemData = (ItemLevelUpData)levelUpData.GetLevelUpData(level);
        return itemData.levelUpDescription;
    }

    public override void LevelEquipmentUp(int level)
    {
        SetLevel(level);
        ItemLevelUpData itemData = (ItemLevelUpData)levelUpData.GetLevelUpData(level);
        m_damageMultiplier = itemData.effectIncrease;
    }
}
