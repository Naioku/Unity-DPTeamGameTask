using UnityEngine;
using UnityEngine.Pool;

namespace DPTeam.AgentSystem
{
    public class AgentManager : MonoBehaviour
    {
        [SerializeField] private int health = 3;
        [SerializeField] private int attackStrength = 1;
        [SerializeField] private float speed = 5;
        [SerializeField] private int maxPoolSize = 30;
        [SerializeField] private int maxActiveAgentsCount = 30;
        [SerializeField] [Range(3, 5)] private int agentCountOnStart = 5;
        [SerializeField] [Range(2, 6)] private float timeBetweenAgentsSpawning = 2;

        private ObjectPool<AgentController> agentPool;
        private float timeSinceLastAgentSpawn;

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
        
        public void OnDestroy()
        {
            agentPool.Clear();
            Managers.Instance.UpdateManager.UpdateActions.RemoveAction(SpawnAgentInCycle);
        }

        public void StartSpawning()
        {
            for (int i = 0; i < agentCountOnStart; i++)
            {
                SpawnAgent();
            }
            
            Managers.Instance.UpdateManager.UpdateActions.AddAction(SpawnAgentInCycle);
        }

        private void SpawnAgentInCycle()
        {
            if (timeSinceLastAgentSpawn >= timeBetweenAgentsSpawning)
            {
                SpawnAgent();
                timeSinceLastAgentSpawn -= timeBetweenAgentsSpawning;
            }

            timeSinceLastAgentSpawn += Time.deltaTime;
        }

        private bool SpawnAgent()
        {
            if (agentPool.CountActive >= maxActiveAgentsCount)
            {
                Debug.LogWarning($"There cannot be more than {maxActiveAgentsCount} active agents." +
                                 $" Change {nameof(maxActiveAgentsCount)} setting in the Inspector.");
                return false;
            }
            
            agentPool.Get();
            return true;
        }

#region Pooling

        private AgentController CreateAgent()
        {
            AgentController agentController = Managers.Instance.SpawningManager.SpawnLocal<AgentController>(Enums.SpawnableObjects.Agent);
            agentController.OnDeathAction = ReleaseAgent;
            agentController.transform.SetParent(transform);
            return agentController;
        }

        private void OnGetAgent(AgentController agent)
        {
            agent.Initialize(health, attackStrength, speed);
            agent.transform.position = Managers.Instance.GameManager.GameplayVolume.GetRandomPointInsideVolume();
            agent.gameObject.SetActive(true);
        }

        private void OnReleaseAgent(AgentController agent) => agent.gameObject.SetActive(false);
        private void OnDestroyAgent(AgentController agent) =>
            Managers.Instance.SpawningManager.DespawnLocal(agent.gameObject);
        private void ReleaseAgent(AgentController agent) => agentPool.Release(agent);

#endregion

#region Tests

        public void Spawn10Agents()
        {
            for (int i = 0; i < 10; i++)
            {
                if (!SpawnAgent()) break;
            }
        }

#endregion
    }
}