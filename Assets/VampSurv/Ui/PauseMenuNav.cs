using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuNav : MonoBehaviour
{
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject levelUpMenu;
    [SerializeField] private GameObject faderObj;

    private HudFader m_hudFader;
    private EventSystem m_eventSystem;
    
    private void Awake()
    {
        faderObj.SetActive(true);
        m_hudFader = faderObj.GetComponent<HudFader>();
        m_eventSystem = EventSystem.current;
    }

    private void Start()
    {
        Time.timeScale = 0.7f;
        m_hudFader.StartFade(1.2f, true, 2f);
    }

    private void OnEnable()
    {
        PlayerData.OnPlayerDeath += OnDeath;
        PlayerData.OnLevelUp += DoLevelUp;
        PlayerController.OnDeathCamMoved += DeathMenu;
        HudFader.OnFadeComplete += FadeFinished;
    }

    private void OnDisable()
    {
        PlayerData.OnPlayerDeath -= OnDeath;
        PlayerData.OnLevelUp -= DoLevelUp;
        PlayerController.OnDeathCamMoved -= DeathMenu;
        HudFader.OnFadeComplete -= FadeFinished;
    }

    private void FadeFinished(bool fadeIn)
    {
        if(fadeIn)
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.gameObject.SetActive(false);
            deathMenu.gameObject.SetActive(false);
            levelUpMenu.SetActive(false);
            faderObj.SetActive(false);
            hud.gameObject.SetActive(true);
        }
        else
        {
            Cursor.visible = true;
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
    }
    
    private void DoLevelUp()
    {
        levelUpMenu.SetActive(true);
        hud.gameObject.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void QuitGameplay()
    {
        faderObj.SetActive(true);
        m_hudFader.StartFade(1.2f, false, 0.3f);
    }
    
    private void LoadMainMenu()
    {
        m_eventSystem.gameObject.SetActive(false);
        QuitGameplay();
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