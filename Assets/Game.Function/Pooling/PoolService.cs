using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class PoolService : MonoBehaviour
    {
        private readonly Dictionary<GameObject, PoolHolder> _dictionary = new();
        private readonly Dictionary<string, PoolHolder> _addressableDictionary = new();

        public GameObject Spawn(GameObject prefab)
        {
            CreatePoolIfNeed(prefab);

            return _dictionary[prefab].Spawn();
        }

        public void ReleaseAll()
        {
            foreach (PoolHolder entry in _dictionary.Values)
            {
                entry?.Dispose();
            }

            foreach (PoolHolder entry in _addressableDictionary.Values)
            {
                entry?.Dispose();
            }
            
            _dictionary.Clear();
            _addressableDictionary.Clear();
        }

        

        public void DeSpawn(GameObject instance)
        {
            if (!instance.activeSelf)
                return;
            
            PoolIdentifier poolIdentifier = instance.GetComponent<PoolIdentifier>();
            if (poolIdentifier == null)
                instance.SetActive(false);
            else
                poolIdentifier.poolHolder.DeSpawn(instance);
        }

        private void CreatePoolIfNeed(GameObject prefab)
        {
            if (_dictionary.ContainsKey(prefab))
                return;
            
            _dictionary.Add(prefab, new PoolHolder(prefab));
        }

        
    }
}