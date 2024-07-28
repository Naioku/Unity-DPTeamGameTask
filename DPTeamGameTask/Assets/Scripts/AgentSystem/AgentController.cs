using UnityEngine;
using UnityEngine.AI;

namespace DPTeam.AgentSystem
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentController : MonoBehaviour
    {
        private int health;
        private int attackStrength;
        
        private NavMeshAgent navMeshAgent;
        private Vector3 currentDestination;

        public System.Action<AgentController> OnDeathAction { private get; set; }
        
        private void Awake() => navMeshAgent = GetComponent<NavMeshAgent>();

        private void OnEnable() => StartMovement();
        private void OnDisable() => StopMovement();

        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.TryGetComponent<AgentController>(out var hitAgent)) return;

            hitAgent.Attack(attackStrength);
        }

        public void Initialize(int health, int attackStrength, float speed)
        {
            this.health = health;
            this.attackStrength = attackStrength;
            navMeshAgent.speed = speed;
        }

        private void Attack(int attackStrength)
        {
            if (health == 0) return;
            
            health = Mathf.Max(0, health - attackStrength);
            if (health == 0)
            {
                PerformDeath();
            }
        }

        private void PerformDeath() => OnDeathAction.Invoke(this);

        private void StartMovement()
        {
            SetRandomDestination();
            Managers.Instance.UpdateManager.UpdateActions.AddAction(PerformMovement);
        }
        private void StopMovement() => Managers.Instance.UpdateManager.UpdateActions.RemoveAction(PerformMovement);
        
        private void PerformMovement()
        {
            if (!IsInStoppingDistance()) return;

            SetRandomDestination();
        }

        private void SetRandomDestination()
        {
            currentDestination = Managers.Instance.GameManager.GameplayVolume.GetRandomPointInsideVolume();
            navMeshAgent.SetDestination(currentDestination);
        }

        private bool IsInStoppingDistance() =>
            (currentDestination - transform.position).sqrMagnitude <= Mathf.Pow(navMeshAgent.stoppingDistance, 2);
    }
}