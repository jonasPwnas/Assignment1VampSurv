using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class PersistentAudioPlayer : MonoBehaviour
{
    //Singleton pattern
    private static PersistentAudioPlayer m_instance;
    public static PersistentAudioPlayer Instance => m_instance;
    
    private AudioSource m_audioSource;

    [SerializeField] private bool isGameplay;
    private float m_startPitch = 0.45f;
    private float m_startVolume = 0.01f;
    private float m_normalVolume = 0.7f;
    
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
        m_audioSource = GetComponent<AudioSource>();
        if (isGameplay)
        {
            SetIsGameplay();
        }
    }

    public void OnPlayerDied()
    {
        StartCoroutine(WaitThenFadeVolume(0f, 0.7f));
    }
    
    public void SetIsGameplay()
    {
        if(m_audioSource.isPlaying == false)
        {
            InitializeAudio();
        }
        
        StopCoroutine(CheckMusicTime());
        PitchShiftUp();
    }

    public void SetIsMenu()
    {
        InitializeAudio();
        StartCoroutine(CheckMusicTime());
    }
    
    private void InitializeAudio()
    {
        m_audioSource.Play();
        m_audioSource.volume = m_startVolume;
        m_audioSource.pitch = m_startPitch;
        StartCoroutine(WaitThenFadeVolume(m_normalVolume, 2f));
    }
    
    private void PitchShiftUp()
    {
        StartCoroutine(WaitThenPitchShift(0.9f, 2.5f));
    }
    private void PitchShiftDown()
    {
        StartCoroutine(WaitThenPitchShift(m_startPitch, 2.5f));
    }

    IEnumerator CheckMusicTime()
    {
        if(m_audioSource.time > 11.875f)
        {
            m_audioSource.time = 5.48f;
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator WaitThenFadeVolume(float fadeTarget, float duration)
    {
        yield return new WaitForSecondsRealtime(0.6f);
        m_audioSource.DOFade(fadeTarget, duration).SetEase(Ease.InOutSine);
        yield return new WaitForSecondsRealtime(duration);
        if (m_audioSource.volume < 0.1f)
        {
            m_audioSource.loop = false;
            m_audioSource.Stop();
            m_audioSource.volume = m_normalVolume;
        }
    }
    IEnumerator WaitThenPitchShift(float pitchTarget, float duration)
    {
        yield return new WaitForSecondsRealtime(0.6f);
        m_audioSource.DOPitch(pitchTarget, duration).SetEase(Ease.Linear);
    }
}
