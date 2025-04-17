using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Pooling
{
    public class PoolHolder : IDisposable
    {
        private readonly GameObject _poolKey;

        private ObjectPool<GameObject> _pool;

        public PoolHolder(GameObject prefab)
        {
            _poolKey = prefab;
            CreatePool();
        }

        public void Dispose()
        {
            _pool?.Dispose();
            GC.SuppressFinalize(this);
        }

        #region API

        public GameObject Spawn()
        {
            return _pool.Get();
        }

        public void DeSpawn(GameObject instance)
        {
            _pool.Release(instance);
        }

        #endregion

        #region Logic

        private void CreatePool()
        {
            _pool = new ObjectPool<GameObject>(CreatePooledItem,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject);
        }

        private GameObject CreatePooledItem()
        {
            if (_poolKey == null)
                return null;
            
            var clone = Object.Instantiate(_poolKey);
            var poolHolder = clone.AddComponent<PoolIdentifier>();
            poolHolder.poolHolder = this;

            return clone;
        }

        private void OnTakeFromPool(GameObject objectType)
        {
            if (objectType != null)
                objectType.SetActive(true);
        }

        private void OnReturnedToPool(GameObject objectType)
        {
            if (objectType != null)
                objectType.SetActive(false);
        }

        private void OnDestroyPoolObject(GameObject objectType)
        {
            if (objectType != null)
                Object.Destroy(objectType);
        }

        #endregion
    }
}