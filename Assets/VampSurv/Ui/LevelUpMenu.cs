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
        EquipmentBase currentRando1 = playerData.GetRandomWeapon();
        EquipmentBase currentRando2 = playerData.GetRandomWeapon();
        EquipmentBase newRando1 = avalibleEquipment.avalibleEquipment[Random.Range(0, avalibleEquipment.avalibleEquipment.Count)].
            GetComponent<EquipmentBase>();
        EquipmentBase newRando2 = avalibleEquipment.avalibleEquipment[Random.Range(0, avalibleEquipment.avalibleEquipment.Count)].
            GetComponent<EquipmentBase>();
        
        switch ((int)playerData.currentLevel)
        {
            case 2:
                levelUpOptions[0].SetOption(currentRando1.levelUpImage, currentRando1.EquipmentName, "Greatness awaits");
                levelUpOptions[1].SetOption(newRando1.levelUpImage, newRando1.EquipmentName, "Greatness awaits even moar");
                levelUpOptions[2].SetOption(newRando2.levelUpImage, newRando2.EquipmentName, "!!??!?");
                break;
            case 3:
                levelUpOptions[0].SetOption(currentRando1.levelUpImage, currentRando1.EquipmentName, "Greatness awaits");
                levelUpOptions[1].SetOption(newRando1.levelUpImage, newRando1.EquipmentName, "Greatness awaits even moar");
                levelUpOptions[2].SetOption(newRando2.levelUpImage, newRando2.EquipmentName, "!!??!?");
                break;
            case 4:
                levelUpOptions[0].SetOption(currentRando1.levelUpImage, currentRando1.EquipmentName, "Greatness awaits");
                levelUpOptions[1].SetOption(newRando1.levelUpImage, newRando1.EquipmentName, "Greatness awaits even moar");
                levelUpOptions[2].SetOption(newRando2.levelUpImage, newRando2.EquipmentName, "!!??!?");
                break;
            case 5:
                levelUpOptions[0].SetOption(currentRando1.levelUpImage, currentRando1.EquipmentName, "Greatness awaits");
                levelUpOptions[1].SetOption(newRando1.levelUpImage, newRando1.EquipmentName, "Greatness awaits even moar");
                levelUpOptions[2].SetOption(newRando2.levelUpImage, newRando2.EquipmentName, "!!??!?");
                break;
        }
    }

    public void FinishedLevelUp()
    {
        pauseMenu.Continue();
    }
}
