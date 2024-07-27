using UnityEngine;

namespace DPTeam
{
    [System.Serializable]
    public class GameplayVolume : MonoBehaviour
    {
        [SerializeField] private Color gizmosColor = new (0, 1, 0, 0.4f);
        
        private Vector3 Center => transform.position;
        private Vector3 Size => transform.lossyScale;
        private Vector3 HalfScale => transform.lossyScale / 2;
        private Vector3 MinPosition => transform.position - HalfScale;
        private Vector3 MaxPosition => transform.position + HalfScale;

        public void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawCube(Center, Size);
        }

        public Vector3 GetRandomPointInsideVolume() => new(
            MinPosition.x + Random.Range(0f, 1f) * Size.x,
            MinPosition.y + Random.Range(0f, 1f) * Size.y,
            MinPosition.z + Random.Range(0f, 1f) * Size.z);
    }
}