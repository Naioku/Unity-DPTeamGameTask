using DPTeam.AgentSystem;
using DPTeam.InteractionSystem;
using DPTeam.SpawningSystem;
using DPTeam.UI;
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

        private GameObject gameBoard;
        private AgentManager agentManager;
        private bool isGameRuning;
        private Menu menu;

        public void Awake()
        {
            interactionController.Awake(Camera.main);
            menu = Managers.Instance.SpawningManager.SpawnLocal<Menu>(Enums.SpawnableObjects.Menu);
        }

        public void OnDestroy() => interactionController.OnDestroy();

        public void StartGame()
        {
            if (isGameRuning)
            {
                Debug.LogWarning("Game is currently running!");
                return;
            }

            isGameRuning = true;
            SpawnWorld();
            Managers.Instance.InputManager.GlobalMap.OnShowMenuData.Performed += StopGame;
            Managers.Instance.InputManager.GlobalMap.Enable();
            interactionController.StartInteracting();
            agentManager = Managers.Instance.SpawningManager.SpawnLocal<AgentManager>(Enums.SpawnableObjects.AgentManager);
            agentManager.StartSpawning();
        }

        public void StopGame()
        {
            interactionController.StopInteracting();
            DespawnWorld();
            Managers.Instance.SpawningManager.DespawnLocal(agentManager.gameObject);
            Managers.Instance.InputManager.GlobalMap.OnShowMenuData.Performed -= StopGame;
            menu.Show();
            isGameRuning = false;
        }

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
            gameBoard = spawningManager.SpawnLocal(Enums.SpawnableObjects.GameBoard);
            gameBoard.transform.localScale = new Vector3(gameBoardSize.x, 1, gameBoardSize.y);

            GameplayVolume = spawningManager.SpawnLocal<GameplayVolume>(Enums.SpawnableObjects.GameplayVolume);
            GameplayVolume.transform.localScale = gameplayVolumeSize;
        }

        private void DespawnWorld()
        {
            SpawningManager<Enums.SpawnableObjects> spawningManager = Managers.Instance.SpawningManager;
            spawningManager.DespawnLocal(gameBoard);
            spawningManager.DespawnLocal(GameplayVolume.gameObject);
        }
    }
}