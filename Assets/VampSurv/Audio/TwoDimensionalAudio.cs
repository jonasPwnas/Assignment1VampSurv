using System;
using UnityEngine;

public class TwoDimensionalAudio : MonoBehaviour
{
    [SerializeField] private AudioClip onHover;
    [SerializeField] private AudioClip onAccept;
    [SerializeField] private AudioClip onGainXp;
    [SerializeField] private AudioClip onLevelUp;
    [SerializeField] private AudioClip onEquipChoice;
    [SerializeField] private AudioClip onDeath;
    [SerializeField] private AudioClip onHeal;
    
    private AudioSource m_audioSource;
    private float m_menuVolume = 0.5f;
    private float m_gameplayVolume = 0.9f;

    private void OnEnable()
    {
        PlayerData.OnPlayerDeath += PlayOnDeath;
        PlayerData.OnLevelUp += PlayOnLevelUp;
        PlayerData.OnDidGainXp += PlayOnGainXp;
        PlayerData.OnFinishedLevelUp += PlayOnEquipChoice;
        PlayerData.OnDidHeal += PlayHeal;
    }
    
    private void OnDisable()
    {
        PlayerData.OnPlayerDeath -= PlayOnDeath;
        PlayerData.OnLevelUp -= PlayOnLevelUp;
        PlayerData.OnDidGainXp -= PlayOnGainXp;
        PlayerData.OnFinishedLevelUp -= PlayOnEquipChoice;
        PlayerData.OnDidHeal -= PlayHeal;
    }

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    private void PlayHeal()
    {
        m_audioSource.PlayOneShot(onHeal, m_gameplayVolume);
    }
    
    public void PlayOnHover()
    {
        m_audioSource.PlayOneShot(onHover, m_menuVolume);
    }
    public void PlayOnAccept()
    {
        m_audioSource.PlayOneShot(onAccept, m_menuVolume);
    }
    public void PlayOnEquipChoice()
    {
        m_audioSource.PlayOneShot(onEquipChoice, m_menuVolume);
    }
    public void PlayOnGainXp()
    {
        m_audioSource.PlayOneShot(onGainXp, m_gameplayVolume);
    }
    public void PlayOnLevelUp()
    {
        m_audioSource.PlayOneShot(onLevelUp, m_gameplayVolume);
    }
    public void PlayOnDeath()
    {
        m_audioSource.PlayOneShot(onDeath, m_gameplayVolume);
    }
}
