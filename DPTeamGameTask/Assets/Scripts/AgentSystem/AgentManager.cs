using UnityEngine;
using UnityEngine.Pool;

namespace DPTeam.AgentSystem
{
    [System.Serializable]
    public class AgentManager
    {
        [SerializeField] private int health = 3;
        [SerializeField] private int attackStrength = 1;
        [SerializeField] private float speed = 5;
        [SerializeField] private int maxPoolSize = 30;
        [SerializeField] [Range(3, 5)] private int agentCountOnStart = 5;

        private ObjectPool<AgentController> agentPool;

        public void Awake()
        {
            agentPool = new ObjectPool<AgentController>(
                CreateAgent,
                OnGetAgent,
                OnReleaseAgent,
                OnDestroyAgent,
                maxSize: maxPoolSize
            );
        }

        public void StartSpawning()
        {
            for (int i = 0; i < agentCountOnStart; i++)
            {
                agentPool.Get();
            }
        }

        private AgentController CreateAgent()
        {
            AgentController agentController = Managers.Instance.SpawningManager.CreateObject<AgentController>(Enums.SpawnableObjects.Agent);
            agentController.OnDeathAction = ReleaseAgent;
            return agentController;
        }

        private void OnGetAgent(AgentController agent)
        {
            agent.Initialize(health, attackStrength, speed);
            agent.transform.position = Managers.Instance.GameManager.GameplayVolume.GetRandomPointInsideVolume();
            agent.gameObject.SetActive(true);
        }
        
        private void OnReleaseAgent(AgentController agent) => agent.gameObject.SetActive(false);
        private void OnDestroyAgent(AgentController agent) => Object.Destroy(agent.gameObject);
        private void ReleaseAgent(AgentController agent) => agentPool.Release(agent);
    }
}