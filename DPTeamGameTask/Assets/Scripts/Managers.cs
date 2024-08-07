using DPTeam.InputSystem;
using DPTeam.SpawningSystem;
using DPTeam.UpdateSystem;
using DPTeam.UpdateSystem.CoroutineSystem;
using UnityEngine;

namespace DPTeam
{
    public class Managers : MonoBehaviour
    {
        public static Managers Instance { get; private set; }

        [field: SerializeField] public GameManager GameManager { get; private set; }
        [field: SerializeField] public SpawningManager<Enums.SpawnableObjects> SpawningManager { get; private set; }
        public UpdateManager UpdateManager { get; private set; }
        public CoroutineManager CoroutineManager { get; private set; }
        public InputManager InputManager { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            UpdateManager = new UpdateManager();
            CoroutineManager = new CoroutineManager();
            InputManager = new InputManager();
            
            CoroutineManager.Awake();
            SpawningManager.Awake();
            InputManager.Awake();
            GameManager.Awake();
        }

        private void OnDestroy()
        {
            GameManager.OnDestroy();
            InputManager.OnDestroy();
            CoroutineManager.OnDestroy();
        }

        private void Update() => UpdateManager.UpdateActions.InvokeActions();
        private void FixedUpdate() => UpdateManager.FixedUpdateActions.InvokeActions();
        private void LateUpdate() => UpdateManager.LateUpdateActions.InvokeActions();

#region Tests

        // Todo: Tests. Delete after UI implementation.
        [ContextMenu("StartGame")]
        private void StartGame() => GameManager.StartGame();

        [ContextMenu("QuitGame")]
        private void QuitGame() => GameManager.QuitGame();

#endregion
    }
}