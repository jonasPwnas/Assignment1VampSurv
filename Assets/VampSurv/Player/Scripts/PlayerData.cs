using System;
using System.Collections;
using Enemies;
using UnityEngine;

public class PlayerData : MonoBehaviour, IGainXp, IHealthAndDamage
{
    //Delegates
    public delegate void TimeThresholdReached(int timeThresholdIndex);
    public static event TimeThresholdReached OnTimeThresholdReached;

    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;
    
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
    private bool m_isDead;
    private int m_sessionKills = 0;

    private void OnEnable()
    {
        SimpleEnemyController.OnEnemyDeath += EnemyDied;
    }

    private void OnDisable()
    {
        SimpleEnemyController.OnEnemyDeath -= EnemyDied;
    }


    private void EnemyDied()
    {
        m_sessionKills++;
        hudUpdater.UpdateKills(m_sessionKills);
    }
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
        }
    }

    public void TakeDamage(float damage, Vector3 hitDirection)
    {
        if(!m_isDead)
        {
            m_currentHealth -= damage;
            hudUpdater.UpdateHealthBar(m_currentHealth, maxHealth);
            damageIndicator.PlayDamageEffect();
            if (m_currentHealth < 0)
            {
                OnPlayerDeath?.Invoke();
                m_isDead = true;
            }
        }
    }

    public void Heal(float healAmount)
    {
        m_currentHealth += healAmount;
        hudUpdater.UpdateHealthBar(m_currentHealth, maxHealth);
    }
    
    IEnumerator UpdateTimeEverySecond()
    {
        int thirtySecMarker;
        yield return new WaitForSeconds(1f);
        m_currentTime++;
        float testTime = m_currentTime % 30f;

        if (testTime < 0.1)
        {
            thirtySecMarker = (int) m_currentTime / 30;
            OnTimeThresholdReached?.Invoke(thirtySecMarker);
        }
        
        hudUpdater.DisplayNewTime(m_currentTime);
        StartCoroutine(UpdateTimeEverySecond());
    }
}