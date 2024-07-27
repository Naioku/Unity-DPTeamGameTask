using UnityEngine;
using UnityEngine.AI;

namespace DPTeam.AgentSystem
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentController : MonoBehaviour
    {
        [SerializeField] private int health = 3;
        [SerializeField] private int attackStrength = 1;
        [SerializeField] private float speed = 5;

        private NavMeshAgent navMeshAgent;
        private Vector3 currentDestination;
        
        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = speed;
        }

        private void OnEnable() => StartMovement();
        private void OnDisable() => StopMovement();

        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.TryGetComponent<AgentController>(out var hitAgent)) return;

            hitAgent.Attack(attackStrength);
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

        private void PerformDeath() => Debug.Log($"{name}: PerformDeath");

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