using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelUpMenu : MonoBehaviour
{
    [SerializeField] private PauseMenuNav pauseMenu;
    [SerializeField] private List<LevelUpOption> levelUpOptions;
    [SerializeField] private AvalibleEquipment avalibleStartEquipment;
    [SerializeField] private RectTransform m_bgRectTransform;
    private PlayerData playerData;
    private List<GameObject> m_avalibleEquipmentList = new List<GameObject>();
    

    private void OnEnable()
    {
        if (playerData == null)
            playerData = FindFirstObjectByType<PlayerData>();

        if ((int)playerData.currentLevel == 2)
        {
            //New solution
            foreach (var equip in avalibleStartEquipment.avalibleEquipment)
            {
                m_avalibleEquipmentList.Add(equip);
            }
        }
        
        LevelUpOption.OnOptionSelected += RemoveChosenEquipment;
        
        Time.timeScale = 0f;
        
        m_bgRectTransform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        m_bgRectTransform.DOScale(1f, 0.7f).SetEase(Ease.OutBack);
        
        SetLevelUptions();
    }

    public void AnimateAndDisable(float animationTime)
    {
        StartCoroutine(AnimateAndDisableMenu(animationTime));
    }
    
    private void OnDisable()
    {
        LevelUpOption.OnOptionSelected -= RemoveChosenEquipment;
        Time.timeScale = 1f;
    }

    private void RemoveChosenEquipment(GameObject equipment)
    {
        if(m_avalibleEquipmentList.Exists(equipment => equipment == equipment))
        {
            //Is the fix really to animate the ui object? yes, probably.
            print("Removed: "+ equipment.name + " from avalible weapons list");
            m_avalibleEquipmentList.Remove(equipment);
        }
    }
    
    private void SetLevelUptions()
    {
        List<GameObject> equipmentList = new List<GameObject>();
        foreach (var equip in m_avalibleEquipmentList)
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
    
    IEnumerator AnimateAndDisableMenu(float duration)
    {
        m_bgRectTransform.DOScale(0.0001f, duration).SetEase(Ease.InBack);
        yield return new WaitForSecondsRealtime(duration);
        gameObject.SetActive(false);
    }
    
}
