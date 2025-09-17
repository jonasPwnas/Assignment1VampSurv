using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuNav : MonoBehaviour
{
    public void LoadGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ScoreMenu()
    {
        Debug.Log("Tried Enter Score Menu");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
