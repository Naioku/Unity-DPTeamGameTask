using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DPTeam.SpawningSystem
{
    [Serializable]
    public class SpawnManager<TKey> where TKey : Enum
    {
        [SerializeField] private List<SpawnableItem<TKey>> spawnableItems;

        private readonly Dictionary<TKey, SpawnableItem<TKey>> entityItemsLookup = new();

        public void Awake()
        {
            foreach (SpawnableItem<TKey> spawnableItem in spawnableItems)
            {
                entityItemsLookup.Add(spawnableItem.Name, spawnableItem);
            }
        }
        
        public GameObject CreateObject(TKey name) =>
            Object.Instantiate(entityItemsLookup[name].GetPrefab());

        public GameObject CreateObject(TKey name, Vector3 position, Quaternion rotation) =>
            Object.Instantiate(entityItemsLookup[name].GetPrefab(), position, rotation);
    }
}