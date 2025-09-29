using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuNav : MonoBehaviour
{
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject levelUpMenu;

    private void OnEnable()
    {
        PlayerData.OnPlayerDeath += DeathMenu;
        PlayerData.OnLevelUp += DoLevelUp;
    }

    private void OnDisable()
    {
        PlayerData.OnPlayerDeath -= DeathMenu;
        PlayerData.OnLevelUp -= DoLevelUp;
    }

    private void DoLevelUp()
    {
        levelUpMenu.SetActive(true);
        hud.gameObject.SetActive(false);
        Cursor.visible = true;
    }

    public void LoadMainMenu()
    {
        Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        
    }

    public void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);
        hud.gameObject.SetActive(false);
        Time.timeScale = 0;
        Cursor.visible = true;
    }
    
    public void DeathMenu()
    {
        deathMenu.gameObject.SetActive(true);
        hud.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        Cursor.visible = true;
    }
    
    public void Continue()
    {
        levelUpMenu.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        hud.gameObject.SetActive(true);
        Time.timeScale = 1;
        Cursor.visible = false;
    }
}