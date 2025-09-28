using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particleSystems;
    [SerializeField] private float effectTimer = 0.3f;

    private int m_effectIndex;
    private float m_startTime;
    private float m_lastEffectTIme;

    private void Start()
    {
        m_startTime = Time.time;
        m_lastEffectTIme = Time.time;
    }

    public void PlayDamageEffect()
    {
        if (m_lastEffectTIme > m_startTime + 0.6)
        {
            CycleAndPlayEffect();
            m_lastEffectTIme = Time.time;
        }
        
        if (Time.time > m_startTime + effectTimer)
        {
            m_startTime = Time.time;
            CycleAndPlayEffect();
            m_lastEffectTIme = Time.time;
        }
    }
    
    private void CycleAndPlayEffect()
    {
        if (m_effectIndex >= particleSystems.Count -1)
        {
            m_effectIndex = 0;
        }
        else
        {
            m_effectIndex++;
        }
        particleSystems[m_effectIndex].Play();
    }
}