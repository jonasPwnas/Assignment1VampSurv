using System;
using UnityEngine;

public class PlayerWorldAudio : MonoBehaviour
{
    [SerializeField] private AudioClip swordStrike;
    
    private AudioSource m_audioSource;
    private float m_volume = 0.6f;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void OnSwordHit()
    {
        m_audioSource.PlayOneShot(swordStrike, m_volume);
    }
}
