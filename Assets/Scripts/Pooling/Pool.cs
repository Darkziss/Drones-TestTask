using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class Pool
    {
        private GameObject _poolObject;

        private readonly Queue<GameObject> _pool = new();

        private bool HaveObjectsInPool => _pool.Count > 0;

        private const bool worldPositionStays = false;

        public void Init(string name, Transform poolParent)
        {
            _poolObject = new(name);

            _poolObject.transform.SetParent(poolParent);
        }

        public GameObject Get(GameObject original, Vector3 position, Quaternion rotation)
        {
            if (HaveObjectsInPool)
            {
                GameObject gameObject = _pool.Dequeue();

                gameObject.transform.SetParent(null);
                gameObject.transform.SetPositionAndRotation(position, rotation);
                gameObject.SetActive(true);

                return gameObject;
            }
            else
            {
                GameObject gameObject = Object.Instantiate(original, position, rotation);

                return gameObject;
            }
        }

        public void Put(GameObject gameObject)
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(_poolObject.transform, worldPositionStays);

            _pool.Enqueue(gameObject);
        }
    }
}