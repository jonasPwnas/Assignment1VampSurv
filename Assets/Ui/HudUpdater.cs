using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudUpdater : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image xpBar;
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text currentTimeText;
    
    public void UpdateHealthBar(float health, float maxHealth)
    {
        healthBar.fillAmount = health / maxHealth;
    }
    
    public void UpdateXpBar(float currentXp, float currentMaxXp, float currentLevel)
    {
        xpBar.fillAmount = currentXp / currentMaxXp;
        currentLevelText.text = "Level " + currentLevel;
    }
    
    //TimeyWimeyStuff
    public void DisplayNewTime(float timeToDisplay)
    {
        float minutesTime = Mathf.FloorToInt(timeToDisplay / 60);
        float secondsTime = Mathf.FloorToInt(timeToDisplay % 60);
        
        currentTimeText.text = minutesTime.ToString("00") + ":" + secondsTime.ToString("00");
    }
}
