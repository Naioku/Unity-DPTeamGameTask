using UnityEngine;

namespace DPTeam
{
    [System.Serializable]
    public class GameManager
    {
        public void StartGame()
        {
            Debug.Log("StartGame");
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}