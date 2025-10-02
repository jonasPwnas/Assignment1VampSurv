using UnityEngine;

public class TwoDimensionalAudio : MonoBehaviour
{
    [SerializeField] private AudioClip onHover;
    [SerializeField] private AudioClip onAccept;
    [SerializeField] private AudioClip onGainXp;
    [SerializeField] private AudioClip onLevelUp;
    [SerializeField] private AudioClip onDeath;
    
    private AudioSource m_audioSource;
    private float m_commonVolume;
    
    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_commonVolume = 0.3f;
    }
    
    public void PlayOnHover()
    {
        m_audioSource.PlayOneShot(onHover, m_commonVolume);
    }
    public void PlayOnAccept()
    {
        m_audioSource.PlayOneShot(onAccept, m_commonVolume);
    }
    public void PlayOnGainXp()
    {
        m_audioSource.PlayOneShot(onGainXp, m_commonVolume);
    }
    public void PlayOnLevelUp()
    {
        m_audioSource.PlayOneShot(onLevelUp, m_commonVolume);
    }
    public void PlayOnDeath()
    {
        m_audioSource.PlayOneShot(onDeath, m_commonVolume);
    }
}
