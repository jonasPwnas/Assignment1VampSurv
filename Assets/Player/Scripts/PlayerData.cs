using System.Collections;
using UnityEngine;

public class PlayerData : MonoBehaviour, IGainXp, IHealthAndDamage
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] HudUpdater hudUpdater;
    [SerializeField] private ProgressionData progressionData;
    [SerializeField] private DamageIndicator damageIndicator;
    [SerializeField] private DamageIndicator xpIndicator;
    
    public float m_currentLevel = 1;
    private float m_currentHealth;
    private float m_currentXp;
    private float m_currentMaxXp;
    private float m_currentTime = 0f;
    
    void Start()
    {
        m_currentHealth = maxHealth;
        hudUpdater.UpdateHealthBar(m_currentHealth, maxHealth);
        StartCoroutine(UpdateTimeEverySecond());
    }
    
    public void GainXp(float xp)
    {
        m_currentXp += xp;
        m_currentMaxXp = progressionData.GetXpForLevel(m_currentLevel);
        xpIndicator.PlayDamageEffect();

        if (m_currentXp >= m_currentMaxXp)
        {
            m_currentXp -= m_currentMaxXp;
            m_currentLevel++; //DING DING DING
            m_currentMaxXp = progressionData.GetXpForLevel(m_currentLevel);
            hudUpdater.UpdateXpBar(m_currentXp, m_currentMaxXp, m_currentLevel);
            m_currentXp = 0;
        }
        else
        {
            hudUpdater.UpdateXpBar(m_currentXp, m_currentMaxXp, m_currentLevel);
            //Blaaaaaaaablablabla
        }
    }

    public void TakeDamage(float damage, Vector3 hitDirection)
    {
        m_currentHealth -= damage;
        hudUpdater.UpdateHealthBar(m_currentHealth, maxHealth);
        damageIndicator.PlayDamageEffect();
    }

    public void Heal(float healAmount)
    {
        m_currentHealth += healAmount;
        hudUpdater.UpdateHealthBar(m_currentHealth, maxHealth);
    }
    
    IEnumerator UpdateTimeEverySecond()
    {
        yield return new WaitForSeconds(1f);
        m_currentTime++;
        hudUpdater.DisplayNewTime(m_currentTime);
        StartCoroutine(UpdateTimeEverySecond());
    }
}