using System;
using UnityEngine;

public class EquipmentBase : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerData.OnPlayerDeath += StopEquipment;
    }

    private void OnDisable()
    {
        PlayerData.OnPlayerDeath -= StopEquipment;
    }
    
    public virtual void StartEquipment()
    {
        //Cool base implementation
    }

    public virtual void LevelEquipmentUp()
    {
        //Also a very cool base implementation
    }

    public virtual void StopEquipment()
    {
        StopAllCoroutines();
    }
}
