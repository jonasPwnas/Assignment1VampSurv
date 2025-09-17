using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudUpdater : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image xpBar;
    [SerializeField] private TMP_Text currentLevelText;
    
    public void UpdateHealthBar(float health, float maxHealth)
    {
        healthBar.fillAmount = health / maxHealth;
    }
    
    public void UpdateXpBar(float currentXp, float currentMaxXp, float currentLevel)
    {
        xpBar.fillAmount = currentXp / currentMaxXp;
        currentLevelText.text = "Level " + currentLevel;
    }
}
