using System;
using UnityEngine;

public class LevelUpMenu : MonoBehaviour
{
    [SerializeField] private PauseMenuNav pauseMenu;
    
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void FinishedLevelUp()
    {
        pauseMenu.Continue();
    }
}
