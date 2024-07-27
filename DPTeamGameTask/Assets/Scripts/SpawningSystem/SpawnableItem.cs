using UnityEngine;

namespace DPTeam.SpawningSystem
{
    public abstract class SpawnableItem<TName> : ScriptableObject where TName : System.Enum
    {
        public abstract TName Name { get; }
        public abstract GameObject GetPrefab();
    }
}