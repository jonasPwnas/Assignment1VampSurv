using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelUpMenu : MonoBehaviour
{
    [SerializeField] private PauseMenuNav pauseMenu;
    [SerializeField] private List<LevelUpOption> levelUpOptions;
    [SerializeField] private AvalibleEquipment avalibleEquipment;
    
    private PlayerData playerData;
    

    private void OnEnable()
    {
        Time.timeScale = 0f;
        if (playerData == null)
            playerData = FindFirstObjectByType<PlayerData>();
        
        SetLevelUptions();
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    private void SetLevelUptions()
    {
        List<GameObject> equipmentList = new List<GameObject>();
        foreach (var equip in avalibleEquipment.avalibleEquipment)
        {
            equipmentList.Add(equip);
        }
        foreach (var equip in playerData.currentEquipment)
        {
            equipmentList.Add(equip);
        }
        
        EquipmentBase rando1 = equipmentList[Random.Range(0, equipmentList.Count)].GetComponent<EquipmentBase>();
        equipmentList.Remove(rando1.gameObject);
        EquipmentBase rando2 = equipmentList[Random.Range(0, equipmentList.Count)].GetComponent<EquipmentBase>();
        equipmentList.Remove(rando2.gameObject);
        EquipmentBase rando3 = equipmentList[Random.Range(0, equipmentList.Count)].GetComponent<EquipmentBase>();
        
        levelUpOptions[0].SetOption(rando1.levelUpImage, rando1.equipmentName, 
            rando1.GetLevelUpDescription(rando1.GetLevel()+1), rando1.gameObject);
        
        levelUpOptions[1].SetOption(rando2.levelUpImage, rando2.equipmentName, 
            rando2.GetLevelUpDescription(rando2.GetLevel()+1), rando2.gameObject);
        
        levelUpOptions[2].SetOption(rando3.levelUpImage, rando3.equipmentName, 
            rando3.GetLevelUpDescription(rando3.GetLevel()+1), rando3.gameObject);
    }

    public void FinishedLevelUp()
    {
        pauseMenu.Continue();
    }
}
