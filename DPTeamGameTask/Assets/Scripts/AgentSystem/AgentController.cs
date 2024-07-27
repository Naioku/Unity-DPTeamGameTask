using UnityEngine;

namespace DPTeam.AgentSystem
{
    [RequireComponent(
        typeof(CapsuleCollider),
        typeof(Rigidbody))]
    public class AgentController : MonoBehaviour
    {
        [SerializeField] private int health = 3;
        [SerializeField] private int attackStrength = 1;

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

        private void PerformDeath()
        {
            Debug.Log($"{name}: PerformDeath");
        }
    }
}