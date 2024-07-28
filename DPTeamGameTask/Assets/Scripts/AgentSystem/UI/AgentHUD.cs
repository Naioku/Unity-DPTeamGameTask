using TMPro;
using UnityEngine;

namespace DPTeam.AgentSystem.UI
{
    public class AgentHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameDynamicLabel;
        [SerializeField] private TextMeshProUGUI healthDynamicLabel;
        
        public void SetAgentInfo(AgentController agent)
        {
            nameDynamicLabel.text = agent.name;
            SetHealth(agent.Health);
        }

        public void SetHealth(int health) => healthDynamicLabel.text = health.ToString();

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}