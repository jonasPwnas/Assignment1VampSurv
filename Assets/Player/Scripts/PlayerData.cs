using UnityEngine;

public class PlayerData : MonoBehaviour, IGainXp, IHealthAndDamage
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] HudUpdater hudUpdater;
    [SerializeField] private ProgressionData progressionData;
    
    
    public float m_currentLevel = 1;
    private float m_currentHealth;
    private float m_currentXp;
    private float m_currentMaxXp;
    
    void Start()
    {
        m_currentHealth = maxHealth;
        hudUpdater.UpdateHealthBar(m_currentHealth, maxHealth);
    }
    
    public void GainXp(float xp)
    {
        m_currentXp += xp;
        m_currentMaxXp = progressionData.GetXpForLevel(m_currentLevel);

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
        }
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
