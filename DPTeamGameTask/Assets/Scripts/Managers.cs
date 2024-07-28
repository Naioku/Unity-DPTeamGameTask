using DPTeam.AgentSystem;
using DPTeam.SpawningSystem;
using DPTeam.UpdateSystem;
using UnityEngine;

namespace DPTeam
{
    public class Managers : MonoBehaviour
    {
        public static Managers Instance { get; private set; }

        [field: SerializeField] public GameManager GameManager { get; private set; }
        [field: SerializeField] public SpawnManager<Enums.SpawnableObjects> SpawningManager { get; private set; }
        [field: SerializeField] public AgentManager AgentManager { get; private set; }
        public UpdateManager UpdateManager { get; private set; }

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
            
            AgentManager.Awake();
            SpawningManager.Awake();
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
        
        
        [ContextMenu("Spawn 10 Agents")]
        public void Spawn10Agents() => AgentManager.Spawn10Agents();

#endregion
    }
}