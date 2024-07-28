using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DPTeam.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button startGameBtn;
        [SerializeField] private Button markoPoloBtn;
        [SerializeField] private TextMeshProUGUI numberLabel;
        [SerializeField] private TextMeshProUGUI textLabel;

        private void Awake()
        {
            markoPoloBtn.onClick.AddListener(StartMarkoPolo);
            startGameBtn.onClick.AddListener(StartGame);
        }

        public void Show() => gameObject.SetActive(true);
        private void Hide() => gameObject.SetActive(false);

        private void StartGame()
        {
            Managers.Instance.GameManager.StartGame();
            Hide();
        }

        private void StartMarkoPolo()
        {
            int randomNumber = Random.Range(0, 101);
            string result = "";
            
            if (randomNumber % 3 == 0)
            {
                result += "Marko";
            }

            if (randomNumber % 5 == 0)
            {
                result += "Polo";
            }

            numberLabel.text = randomNumber.ToString();
            textLabel.text = result;
        }
    }
}