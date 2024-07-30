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
        // Because of using Unity's Plane mesh.
        private const int GameBoardSizeAdjustment = 10;
        
        [SerializeField] public CameraController cameraController;
        [SerializeField] public InteractionController interactionController;
        
        [field: SerializeField] public Settings GameSettings { get; private set; }
        public GameplayVolume GameplayVolume { get; private set; }

        private GameObject gameBoard;
        private AgentManager agentManager;
        private bool isGameRunning;
        private Menu menu;

        public void Awake()
        {
            interactionController.Awake(cameraController);
            menu = Managers.Instance.SpawningManager.SpawnLocal<Menu>(Enums.SpawnableObjects.Menu);
        }

        public void OnDestroy() => interactionController.OnDestroy();

        public void StartGame()
        {
            if (isGameRunning)
            {
                Debug.LogWarning("Game is currently running!");
                return;
            }

            isGameRunning = true;
            SpawnWorld();
            Managers.Instance.InputManager.GlobalMap.OnShowMenuData.Performed += StopGame;
            Managers.Instance.InputManager.GlobalMap.Enable();
            cameraController.StartMovement();
            interactionController.StartInteracting();
            agentManager = Managers.Instance.SpawningManager.SpawnLocal<AgentManager>(Enums.SpawnableObjects.AgentManager);
            agentManager.StartSpawning();
        }

        public void StopGame()
        {
            cameraController.StopMovement();
            interactionController.StopInteracting();
            Managers.Instance.SpawningManager.DespawnLocal(agentManager.gameObject);
            Managers.Instance.InputManager.GlobalMap.OnShowMenuData.Performed -= StopGame;
            DespawnWorld();
            menu.Show();
            isGameRunning = false;
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
            gameBoard.transform.localScale = new Vector3(
                GameSettings.GameBoardSize.x / GameBoardSizeAdjustment,
                1,
                GameSettings.GameBoardSize.y / GameBoardSizeAdjustment);

            GameplayVolume = spawningManager.SpawnLocal<GameplayVolume>(Enums.SpawnableObjects.GameplayVolume);
            GameplayVolume.transform.localScale = GameSettings.GameplayVolumeSize;
        }

        private void DespawnWorld()
        {
            SpawningManager<Enums.SpawnableObjects> spawningManager = Managers.Instance.SpawningManager;
            spawningManager.DespawnLocal(gameBoard);
            spawningManager.DespawnLocal(GameplayVolume.gameObject);
        }
    }
}