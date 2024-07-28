using DPTeam.SpawningSystem;
using UnityEngine;

namespace DPTeam
{
    [CreateAssetMenu(fileName = "SpawnableObject", menuName = "Spawnable items/Create spawnable object", order = 0)]
    public class SpawnableObjectSO : SpawnableItem<Enums.SpawnableObjects>
    {
        [SerializeField] private Enums.SpawnableObjects objName;
        [SerializeField] private GameObject prefab;

        public override Enums.SpawnableObjects Name => objName;
        public override GameObject GetPrefab() => prefab;
    }
}