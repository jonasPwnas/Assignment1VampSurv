using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class PersistentMusicManager : MonoBehaviour
{
    //Singleton pattern
    private static PersistentMusicManager m_instance;
    public static PersistentMusicManager Instance => m_instance;
    
    private AudioSource m_audioSource;
    private AudioMixerGroup m_2dAudioMixerGroup;
    private AudioMixerGroup m_3dAudioMixerGroup;
    private bool m_isGameplay;
    private bool m_didPitchUp;
    
    private void Awake()
    {
        //Singleton pattern
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
        }
        DontDestroyOnLoad(gameObject);
        DOTween.Init();
    }

    private void Start()
    {
        m_2dAudioMixerGroup = GetComponent<AudioSource>().outputAudioMixerGroup;
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.volume = 0f;
        m_audioSource.Play();
        m_audioSource.pitch = 0.45f;
        m_audioSource.DOFade(0.2f, 2f).SetEase(Ease.InOutSine);
    }

    public void SetIsGameplay(bool isGameplay)
    {
        m_isGameplay = isGameplay;
    }
    
    private void Update() //Use coroutine instead?
    {
        if (m_isGameplay && !m_didPitchUp)
        {
            StartCoroutine(PitchShift());
        }
        
        if(!m_isGameplay &&m_audioSource.time > 11.875f)
        {
            m_audioSource.time = 5.48f;
        }
    }
    
    IEnumerator PitchShift()
    {
        m_didPitchUp = true;
        float duration = 2.6f;
        m_audioSource.DOPitch(0.94f, duration).SetEase(Ease.Linear);
        yield return new WaitForSecondsRealtime(duration);
    }
}
