using UnityEngine;
using UnityEngine.UI;

public class EquipmentLevelUpData : ScriptableObject
{
    public virtual object GetLevelUpData(int level)
    {
        Debug.Log("Please implement GetLevelUpData in " + name + "");
        return null;
    }
    
    public virtual int GetMaxLevel()
    {
        return 1;
    }
}