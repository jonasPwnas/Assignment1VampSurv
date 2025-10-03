using System;
using System.Collections;
using Player;
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
        PlayerData.OnPlayerDeath += OnDeath;
        PlayerData.OnLevelUp += DoLevelUp;
        PlayerController.OnDeathCamMoved += DeathMenu;
    }

    private void OnDisable()
    {
        PlayerData.OnPlayerDeath -= OnDeath;
        PlayerData.OnLevelUp -= DoLevelUp;
        PlayerController.OnDeathCamMoved -= DeathMenu;
    }

    private void DoLevelUp()
    {
        levelUpMenu.SetActive(true);
        hud.gameObject.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
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
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    private void OnDeath()
    {
        hud.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 0;
    }
    public void DeathMenu()
    {
        deathMenu.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    public void Continue()
    {
        levelUpMenu.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        hud.gameObject.SetActive(true);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void FinishLevelUp()
    {
        float duration = 0.7f;
        levelUpMenu.GetComponent<LevelUpMenu>().AnimateAndDisable(duration);
        StartCoroutine(ContinueAfterTime(duration));
    }
    
    IEnumerator ContinueAfterTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Continue();
    }
}