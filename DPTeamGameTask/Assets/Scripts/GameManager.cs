using DPTeam.AgentSystem;
using DPTeam.SpawningSystem;
using UnityEngine;

namespace DPTeam
{
    [System.Serializable]
    public class GameManager
    {
        [SerializeField] public Vector2 gameBoardSize;
        [SerializeField] public Vector3 gameplayVolumeSize;

        public GameplayVolume GameplayVolume { get; private set; }

        private bool isGameStarted;

        public void StartGame()
        {
            if (isGameStarted)
            {
                Debug.LogWarning("Game is currently running!");
                return;
            }

            SpawnWorld();

            isGameStarted = true;
            AgentManager agentManager = Managers.Instance.SpawningManager.CreateObject<AgentManager>(Enums.SpawnableObjects.AgentManager);
            agentManager.StartSpawning();
        }

        private void SpawnWorld()
        {
            SpawningManager<Enums.SpawnableObjects> spawningManager = Managers.Instance.SpawningManager;
            GameObject gameBoard = spawningManager.CreateObject(Enums.SpawnableObjects.GameBoard);
            gameBoard.transform.localScale = new Vector3(gameBoardSize.x, 1, gameBoardSize.y);

            GameplayVolume = spawningManager.CreateObject<GameplayVolume>(Enums.SpawnableObjects.GameplayVolume);
            GameplayVolume.transform.localScale = gameplayVolumeSize;
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