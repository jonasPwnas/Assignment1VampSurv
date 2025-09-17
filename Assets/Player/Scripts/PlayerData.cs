using UnityEngine;

public class PlayerData : MonoBehaviour, IGainXp, IHealthAndDamage
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] HudUpdater hudUpdater;
    [SerializeField] private ProgressionData progressionData;
    
    
    public float m_currentLevel = 1;
    private float m_currentHealth;
    private float m_tempXp;
    private float m_currentXp;
    private float m_currentMaxXp;
    
    void Start()
    {
        m_currentHealth = maxHealth;
        hudUpdater.UpdateHealthBar(m_currentHealth, maxHealth);
    }
    
    public void GainXp(float xp)
    {
        m_tempXp = 0f;
        m_currentMaxXp = progressionData.GetXpForLevel(m_currentLevel);
        m_tempXp += m_currentXp + xp;
        m_currentXp += xp;
        
        if (m_tempXp > m_currentMaxXp)
        {
            m_tempXp =- m_currentMaxXp;
            hudUpdater.UpdateXpBar(m_currentXp, m_currentMaxXp, m_currentLevel);
            m_currentXp = 0f;
            m_currentLevel += 1f;
            GainXp(m_tempXp);
            return;
        }
        hudUpdater.UpdateXpBar(m_currentXp, m_currentMaxXp, m_currentLevel);
    }

    public void TakeDamage(float damage, Vector3 hitDirection)
    {
        m_currentHealth -= damage;
        hudUpdater.UpdateHealthBar(m_currentHealth, maxHealth);
    }

    public void Heal(float healAmount)
    {
        m_currentHealth += healAmount;
        hudUpdater.UpdateHealthBar(m_currentHealth, maxHealth);
    }
}
