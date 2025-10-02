using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuNav : MonoBehaviour
{
    [SerializeField] private Image faderImage;

    private PersistentAudioPlayer m_audioPlayer;
    private void Start()
    {
        m_audioPlayer = PersistentAudioPlayer.Instance;
        m_audioPlayer.SetIsMenu();
    }

    public void LoadGameplay()
    {
        m_audioPlayer.SetIsGameplay();
        StartCoroutine(FadeVisibility(false, 0.7f, false));
    }

    public void ScoreMenu()
    {
        Debug.Log("Tried Enter Score Menu");
    }
    
    public void QuitGame()
    {
        StartCoroutine(FadeVisibility(false, 0.7f, true));
    }

    IEnumerator FadeVisibility(bool fadeIn, float duration, bool quitGame)
    {
        if(fadeIn)
        {
            faderImage.DOFade(0f, duration);
        }
        else
        {
            faderImage.DOFade(1f, duration);
        }
        yield return new WaitForSecondsRealtime(duration);

        if (quitGame)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
