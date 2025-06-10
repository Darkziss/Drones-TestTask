using System;
using UnityEngine;
using Pooling;

namespace Drones
{
    public class Resource : MonoBehaviour
    {
        private Transform _transform;

        public bool IsReserved { get; private set; }

        public Vector3 Position => _transform.position;

        private void Awake()
        {
            if (_transform == null)
                _transform = transform;
        }

        public void Reserve()
        {
            if (IsReserved)
                throw new InvalidOperationException(nameof(IsReserved));

            IsReserved = true;
        }

        public void Release()
        {
            if (!IsReserved)
                throw new InvalidOperationException(nameof(IsReserved));

            IsReserved = false;

            PoolStorage.PutToPool(nameof(Resource), this);
        }

        public void Collect()
        {
            gameObject.SetActive(false);
        }
    }
}