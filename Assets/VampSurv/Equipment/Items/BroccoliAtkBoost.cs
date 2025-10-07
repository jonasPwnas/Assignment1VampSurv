using System;
using UnityEngine;
using static EquipmentLevelUp.EquipmentLevelUpStructs;

public class BroccoliAtkBoost : EquipmentBase
{
    public delegate void BroccoliAtkBoostPowerUp(float multiplier);
    public static event BroccoliAtkBoostPowerUp OnBroccoliAtkBoostPowerUp;
    private float m_damageMultiplier = 1.1f;

    private void Start()
    {
        OnBroccoliAtkBoostPowerUp.Invoke(m_damageMultiplier);
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
