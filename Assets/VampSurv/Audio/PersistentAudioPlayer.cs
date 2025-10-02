using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class PersistentAudioManager : MonoBehaviour
{
    //Singleton pattern
    private static PersistentAudioManager m_instance;
    public static PersistentAudioManager Instance => m_instance;
    
    private AudioSource m_audioSource;

    [SerializeField] private bool isGameplay;
    private float m_startPitch = 0.45f;
    
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
        else
        {
            SetIsMenu();
        }
    }

    public void SetIsGameplay()
    {
        if(m_audioSource.isPlaying == false)
        {
            InitializeAudio();
        }
        
        StopCoroutine(CheckMusicTime());
        PitchShift();
    }

    public void SetIsMenu()
    {
        InitializeAudio();
        StartCoroutine(CheckMusicTime());
    }
    
    private void InitializeAudio()
    {
        m_audioSource.Play();
        m_audioSource.volume = 0.01f;
        m_audioSource.pitch = m_startPitch;
        StartCoroutine(WaitThenFadeIn());
    }
    
    private void PitchShift()
    {
        StartCoroutine(WaitThenPitchShift());
    }

    IEnumerator CheckMusicTime()
    {
        if(m_audioSource.time > 11.875f)
        {
            m_audioSource.time = 5.48f;
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator WaitThenFadeIn()
    {
        yield return new WaitForSecondsRealtime(0.6f);
        m_audioSource.DOFade(0.6f, 2f).SetEase(Ease.InOutSine);
    }
    IEnumerator WaitThenPitchShift()
    {
        yield return new WaitForSecondsRealtime(0.6f);
        float duration = 4f;
        m_audioSource.DOPitch(0.9f, duration).SetEase(Ease.Linear);
    }
}
