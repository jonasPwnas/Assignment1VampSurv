using System;
using UnityEngine;

public class EquipmentBase : MonoBehaviour
{
    [SerializeField] public Sprite levelUpImage;
    [SerializeField] public string EquipmentName = "Equipment";
    [HideInInspector] public string levelUpDescription;
    
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

    public virtual void LevelEquipmentUp(int level)
    {
        //Also a very cool base implementation
    }

    public virtual void StopEquipment()
    {
        StopAllCoroutines();
    }
}
