using DPTeam.UpdateSystem;
using UnityEngine;

namespace DPTeam
{
    public class Managers : MonoBehaviour
    {
        public static Managers Instance { get; private set; }

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
        }
        
        private void Update() => UpdateManager.UpdateActions.InvokeActions();
        private void FixedUpdate() => UpdateManager.FixedUpdateActions.InvokeActions();
        private void LateUpdate() => UpdateManager.LateUpdateActions.InvokeActions();
    }
}