using UnityEngine;

namespace DPTeam
{
    [System.Serializable]
    public class Settings
    {
        [field: SerializeField] public Vector2 GameBoardSize { get; private set; }
        [field: SerializeField] public Vector3 GameplayVolumeSize { get; private set; }
    }
}