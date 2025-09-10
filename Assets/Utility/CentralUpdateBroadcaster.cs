using UnityEngine;

namespace Utility
{
    public class CentralUpdateBroadcaster : MonoBehaviour
    {
        public delegate void UpdateAction();
        public static event UpdateAction OnUpdate;

        public delegate void FixedUpdateAction();
        public static event FixedUpdateAction OnFixedUpdate;

        public delegate void LateUpdateAction();
        public static event LateUpdateAction OnLateUpdate;

        private bool bIsPaused = false;

        private void Update()
        {
            if (bIsPaused)
                return;

            OnUpdate.Invoke();
        }

        private void FixedUpdate()
        {
            if (bIsPaused)
                return;

            OnFixedUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            if (bIsPaused)
                return;

            OnLateUpdate?.Invoke();
        }

        private void OnEnable()
        {
            BroadcastGameState.OnPauseGame += Pause;
            BroadcastGameState.OnUnpauseGame += Unpause;
        }
        private void OnDisable()
        {
            BroadcastGameState.OnPauseGame -= Pause;
            BroadcastGameState.OnUnpauseGame -= Unpause;
        }
        private void Pause()
        {
            Time.timeScale = 0f;
        }

        private void Unpause()
        {
            Time.timeScale = 1f;
        }
    }
}