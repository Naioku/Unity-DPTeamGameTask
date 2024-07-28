using UnityEngine;

namespace DPTeam
{
    [System.Serializable]
    public class GameManager
    {
        [field: SerializeField] public GameplayVolume GameplayVolume { get; private set; }

        private bool isGameStarted;

        public void StartGame()
        {
            if (isGameStarted)
            {
                Debug.LogWarning("Game is currently running!");
                return;
            }

            Debug.Log("StartGame");
            isGameStarted = true;
            Managers.Instance.AgentManager.StartSpawning();
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}