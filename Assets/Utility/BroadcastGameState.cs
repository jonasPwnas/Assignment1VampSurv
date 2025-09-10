using UnityEngine;

namespace Utility
{
    public class BroadcastGameState : MonoBehaviour
    {
        public delegate void StartGameplayAction();
        public static event StartGameplayAction OnStartGameplay;

        public delegate void StartMainMenuAction();
        public static event StartMainMenuAction OnStartMenu;

        public delegate void PauseGameAction();
        public static event PauseGameAction OnPauseGame;

        public delegate void UnpauseGameAction();
        public static event UnpauseGameAction OnUnpauseGame;

        public void StartGameplay()
        {
            OnStartGameplay?.Invoke();
        }

        public void StartMenu()
        {
            OnStartMenu?.Invoke();
        }

        public void PauseGame()
        {
            OnPauseGame?.Invoke();
        }

        public void UnpauseGame()
        {
            OnUnpauseGame?.Invoke();
        }
    }
}
