using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerData : MonoBehaviour, IGainXp, IHealthAndDamage
{
    //Delegates
    public delegate void SessionTimeThresholdReached(int sessionTimeThresholdIndex);
    public static event SessionTimeThresholdReached OnSessionTimeThresholdReached;

    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;
    public delegate void PlayerTakeDamage();
    public static event PlayerTakeDamage OnPlayerTakeDamage;
    
    public delegate void LevelUp();
    public static event LevelUp OnLevelUp;
    
    public delegate void FinishedLevelUp();
    public static event FinishedLevelUp OnFinishedLevelUp;
    public delegate void DidGainXp();
    public static event DidGainXp OnDidGainXp;
    public delegate void DidHeal();
    public static event DidHeal OnDidHeal;
    
    //Editor editable
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] HudUpdater hudUpdater;
    [SerializeField] private ProgressionData progressionData;
    [SerializeField] private DamageIndicator damageIndicator;
    [SerializeField] private DamageIndicator xpIndicator;
    [SerializeField] public List<GameObject> currentEquipment;
    [SerializeField] private ParticleSystem healingEffect;
    
    //Public
    [HideInInspector] public float currentLevel = 1;
    
    //Privates
    private float m_currentHealth;
    private float m_currentXp;
    private float m_currentMaxXp;
    private float m_currentTime = 0f;
    private bool m_isDead;
    private int m_sessionKills = 0;
    private PersistentAudioPlayer m_AudioPlayer;

    private void OnEnable()
    {
        SimpleEnemyController.OnEnemyDeath += EnemyDied;
        LevelUpOption.OnOptionSelected += AddOrLevelWeapon;
    }

    private void OnDisable()
    {
        SimpleEnemyController.OnEnemyDeath -= EnemyDied;
        LevelUpOption.OnOptionSelected -= AddOrLevelWeapon;
    }

    public void AddOrLevelWeapon(GameObject newWeapon)
    {
        OnFinishedLevelUp?.Invoke();
        EquipmentBase equipment = newWeapon.GetComponent<EquipmentBase>();
        if (!equipment.GetIsEquipped())
        {
            print("Make New WEAPON!!! AAAAAH");
            GameObject newEquipment;
            newEquipment = Instantiate(newWeapon, transform.position, transform.rotation, transform);
            currentEquipment.Add(newEquipment);
        }
        else
        {
            print("Level up existing weapon POOOWEEEERR!!!");
            equipment.LevelEquipmentUp(equipment.GetLevel()+1);
            if (equipment.GetMaxLevel() == equipment.GetLevel()+1)
            {
                currentEquipment.Remove(equipment.gameObject);
                print("Equipment reached max level, removing from list");
            }
        }
    }

    public GameObject GetRandomWeapon()
    {
        int randomIndex = Random.Range(0, currentEquipment.Count);
        return currentEquipment[randomIndex];
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
        currentEquipment.Add(GetComponentInChildren<EquipmentBase>().gameObject);
        m_AudioPlayer = PersistentAudioPlayer.Instance;
        StartCoroutine(UpdateTimeEverySecond());
    }
    
    public void GainXp(float xp)
    {
        m_currentXp += xp;
        m_currentMaxXp = progressionData.GetXpForLevel(currentLevel);
        xpIndicator.PlayDamageEffect();
        OnDidGainXp?.Invoke();

        if (m_currentXp >= m_currentMaxXp)
        {
            m_currentXp -= m_currentMaxXp;
            currentLevel++; //DING DING DING
            m_currentMaxXp = progressionData.GetXpForLevel(currentLevel);
            hudUpdater.UpdateXpBar(m_currentXp, m_currentMaxXp, currentLevel);
            m_currentXp = 0;
            OnLevelUp?.Invoke();
        }
        else
        {
            hudUpdater.UpdateXpBar(m_currentXp, m_currentMaxXp, currentLevel);
        }
    }

    public void TakeDamage(float damage, Vector3 hitDirection)
    {
        if(!m_isDead)
        {
            m_currentHealth -= damage;
            hudUpdater.UpdateHealthBar(m_currentHealth, maxHealth);
            damageIndicator.PlayDamageEffect();
            OnPlayerTakeDamage?.Invoke();
            if (m_currentHealth < 0)
            {
                damageIndicator.gameObject.SetActive(false);
                OnPlayerDeath?.Invoke();
                m_AudioPlayer.OnPlayerDied();
                m_isDead = true;
            }
        }
    }

    public void Heal(float healAmount)
    {
        print("DID HEAL");
        m_currentHealth += healAmount;
        m_currentHealth = Mathf.Clamp(m_currentHealth, 0, maxHealth);
        hudUpdater.UpdateHealthBar(m_currentHealth, maxHealth);
        OnDidHeal?.Invoke();
        healingEffect.Play();
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
            OnSessionTimeThresholdReached?.Invoke(thirtySecMarker);
        }
        
        hudUpdater.DisplayNewTime(m_currentTime);
        StartCoroutine(UpdateTimeEverySecond());
    }
}