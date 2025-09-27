using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuNav : MonoBehaviour
{
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pauseMenu;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);
        hud.gameObject.SetActive(false);
        Time.timeScale = 0;
    }
    
    public void Continue()
    {
        pauseMenu.gameObject.SetActive(false);
        hud.gameObject.SetActive(true);
        Time.timeScale = 1;
    }
}