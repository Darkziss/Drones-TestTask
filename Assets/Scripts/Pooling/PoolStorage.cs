using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public static class PoolStorage
    {
        private static GameObject poolParent;
        
        private static readonly Dictionary<string, Pool> _pools = new();

        private const string poolParentName = "Pools";

        public static void Init()
        {
            poolParent = new GameObject(poolParentName);
        }

        public static T GetFromPool<T>(string name, T original, Vector3 position, Quaternion rotation) where T: Component
        {
            bool contains = _pools.ContainsKey(name);

            if (!contains)
                CreatePool(name);

            GameObject clone = _pools[name].Get(original.gameObject, position, rotation);

            return clone.GetComponent<T>();
        }

        public static void PutToPool<T>(string name, T clone) where T: MonoBehaviour
        {
            bool contains = _pools.ContainsKey(name);

            if (!contains)
                CreatePool(name);

            _pools[name].Put(clone.gameObject);
        }

        private static void CreatePool(string name)
        {
            Pool pool = new();
            pool.Init($"{name}Pool", poolParent.transform);

            _pools.Add(name, pool);
        }
    }
}