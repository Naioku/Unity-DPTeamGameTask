using System;
using DPTeam.InteractionSystem;
using UnityEngine;
using UnityEngine.AI;

namespace DPTeam.AgentSystem
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentController : MonoBehaviour
    {
        [SerializeField] private Interaction interaction;
        [SerializeField] private Canvas selection;

        private int health;
        private int attackStrength;
        
        private NavMeshAgent navMeshAgent;
        private Vector3 currentDestination;

        public int Health
        {
            get => health;
            private set
            {
                health = value;
                OnHealthChangeAction?.Invoke(health);
            }
        }

        public Action<AgentController> OnDeathAction { private get; set; }
        public Action<AgentController> OnSelectAction { private get; set; }
        public Action<int> OnHealthChangeAction { private get; set; }
        
        private void Awake() => navMeshAgent = GetComponent<NavMeshAgent>();
        private void Start() => interaction.SetAction(Enums.InteractionType.Click, Enums.InteractionState.EnterType, SelectInteraction);

        private void OnEnable() => StartMovement();
        private void OnDisable() => StopMovement();

        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.TryGetComponent<AgentController>(out var hitAgent)) return;
            
            hitAgent.Attack(attackStrength);
        }

        public void Initialize(
            string name,
            int health,
            int attackStrength,
            float speed)
        {
            base.name = name;
            this.Health = health;
            this.attackStrength = attackStrength;
            navMeshAgent.speed = speed;
            selection.gameObject.SetActive(false);
        }
        
        private void SelectInteraction(Interaction.InteractionActionArgs args)
        {
            OnSelectAction.Invoke(this);
            selection.gameObject.SetActive(true);
        }

        private void Attack(int attackStrength)
        {
            if (Health == 0) return;
            
            Health = Mathf.Max(0, Health - attackStrength);
            if (Health == 0)
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
        
        private void StopMovement()
        {
            navMeshAgent.ResetPath();
            Managers.Instance.UpdateManager.UpdateActions.RemoveAction(PerformMovement);
        }

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

        public void Deselect() => selection.gameObject.SetActive(false);
    }
}