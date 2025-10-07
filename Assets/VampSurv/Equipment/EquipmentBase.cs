using System;
using UnityEngine;

public class EquipmentBase : MonoBehaviour
{
    [SerializeField] public EquipmentLevelUpData levelUpData;
    [SerializeField] public Sprite levelUpImage;
    [SerializeField] public string equipmentName = "Equipment";

    private int m_currentLevel = 0;
    private bool m_isEquipped = false;
    
    public virtual void OnEnable()
    {
        PlayerData.OnPlayerDeath += StopEquipment;
    }

    public virtual void OnDisable()
    {
        PlayerData.OnPlayerDeath -= StopEquipment;
    }

    public void SetLevel(int level)
    {
        m_currentLevel = level;
    }
    
    public void SetIsEquipped(bool isEquipped)
    {
        m_isEquipped = isEquipped;
    }
    
    public bool GetIsEquipped()
    {
        return m_isEquipped;
    }
    
    public int GetLevel()
    {
        if (!m_isEquipped)
        {
            return -1;
        }
        else
        {
            return m_currentLevel;
        }
    }

    public int GetMaxLevel()
    {
        return levelUpData.GetMaxLevel();
    }
    
    public virtual string GetLevelUpDescription(int level)
    {
        Debug.Log("Please implement GetDescription in: "+ gameObject.name);
        return "this is great thing, kill big, fight much (you should probably implement this)";
    }
    
    public virtual void LevelEquipmentUp(int level)
    {
        Debug.Log("Please implement LevelEquipmentUp in: " + gameObject.name);
    }

    public virtual void StopEquipment()
    {
        StopAllCoroutines();
    }
}
