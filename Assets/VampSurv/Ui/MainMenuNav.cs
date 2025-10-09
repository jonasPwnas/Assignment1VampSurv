using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuNav : MonoBehaviour
{
    [SerializeField] private GameObject menuObj;
    [SerializeField] private GameObject scoreObj;
    [SerializeField] private Image faderImage;
    private PersistentAudioPlayer m_audioPlayer;
    private EventSystem m_eventSystem;
    
    private void Start()
    {
        m_eventSystem = EventSystem.current;
        faderImage.gameObject.SetActive(true);
        m_audioPlayer = PersistentAudioPlayer.Instance;
        m_audioPlayer.SetIsMenu();
        StartCoroutine(FadeVisibility(true, 0.7f, false, 0.6f));
    }

    public void LoadGameplay()
    {
        m_eventSystem.gameObject.SetActive(false);
        m_audioPlayer.SetIsGameplay();
        StartCoroutine(FadeVisibility(false, 0.7f, false, 0.1f));
    }

    public void ShowScoreMenu()
    {
        menuObj.SetActive(false);
        scoreObj.SetActive(true);
    }
    
    public void ShowMainMenu()
    {
        menuObj.SetActive(true);
        scoreObj.SetActive(false);
    }
    
    public void QuitGame()
    {
        m_eventSystem.gameObject.SetActive(false);
        StartCoroutine(FadeVisibility(false, 0.7f, true, 0.3f));
    }

    IEnumerator FadeVisibility(bool fadeIn, float duration, bool quitGame, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        
        if(fadeIn)
        {
            faderImage.DOFade(0f, duration);
        }
        else
        {
            faderImage.DOFade(1f, duration);
        }
        yield return new WaitForSecondsRealtime(duration + 0.1f);

        if (quitGame)
        {
            Application.Quit();
        }
        if(!quitGame && !fadeIn)
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
