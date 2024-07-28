using DPTeam.AgentSystem;
using DPTeam.InteractionSystem;
using DPTeam.SpawningSystem;
using UnityEngine;

namespace DPTeam
{
    [System.Serializable]
    public class GameManager
    {
        [SerializeField] public Vector2 gameBoardSize;
        [SerializeField] public Vector3 gameplayVolumeSize;
        [SerializeField] public InteractionController interactionController;

        public GameplayVolume GameplayVolume { get; private set; }

        private bool isGameStarted;

        public void Awake() => interactionController.Awake(Camera.main);
        public void OnDestroy() => interactionController.OnDestroy();

        public void StartGame()
        {
            if (isGameStarted)
            {
                Debug.LogWarning("Game is currently running!");
                return;
            }

            isGameStarted = true;
            SpawnWorld();
            Managers.Instance.InputManager.GlobalMap.Enable();
            interactionController.StartInteracting();
            AgentManager agentManager = Managers.Instance.SpawningManager.SpawnLocal<AgentManager>(Enums.SpawnableObjects.AgentManager);
            agentManager.StartSpawning();
        }

        public void StopGame() => interactionController.StopInteracting();

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void SpawnWorld()
        {
            SpawningManager<Enums.SpawnableObjects> spawningManager = Managers.Instance.SpawningManager;
            GameObject gameBoard = spawningManager.SpawnLocal(Enums.SpawnableObjects.GameBoard);
            gameBoard.transform.localScale = new Vector3(gameBoardSize.x, 1, gameBoardSize.y);

            GameplayVolume = spawningManager.SpawnLocal<GameplayVolume>(Enums.SpawnableObjects.GameplayVolume);
            GameplayVolume.transform.localScale = gameplayVolumeSize;
        }
    }
}