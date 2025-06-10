using System;
using System.Collections;
using UnityEngine;

namespace Drones
{
    public class ResourceFinder : MonoBehaviour
    {
        [SerializeField, Range(MinSearchCapacity, MaxSearchCapacity)] private int _searchCapacity;
        [SerializeField, Range(MinSearchRadius, MaxSearchRadius)] private float _searchRadius;

        [SerializeField] private LayerMask _resourceLayer;

        private Transform _transform;

        private Collider[] _foundResources;

        private Action<Resource> _foundCallback;

        private readonly WaitForSeconds _searchDelay = new(SearchDelay);

        private const int MinSearchCapacity = 1;
        private const int MaxSearchCapacity = 30;

        private const float MinSearchRadius = 1f;
        private const float MaxSearchRadius = 100f;

        private const float SearchDelay = 0.1f;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;
        }

        private void Start()
        {
            _foundResources = new Collider[_searchCapacity];
        }

        public void StartSearch(Action<Resource> callback)
        {
            _foundCallback = callback;

            StartCoroutine(TryFindResource());
        }

        private IEnumerator TryFindResource()
        {
            while (true)
            {
                CleanupFoundResources();

                int resourcesCount = Physics.OverlapSphereNonAlloc(_transform.position, _searchRadius,
                    _foundResources, _resourceLayer);

                if (resourcesCount > 0)
                {
                    Resource resource = GetClosestResource();

                    if (resource != null)
                    {
                        _foundCallback?.Invoke(resource);

                        break;
                    }
                }

                yield return _searchDelay;
            }
        }

        private Resource GetClosestResource()
        {
            Resource closestResource = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < _foundResources.Length; i++)
            {
                if (_foundResources[i] == null)
                    break;

                Resource resource = _foundResources[i].GetComponent<Resource>();

                if (!resource.IsReserved)
                {
                    float distance = Vector3.Distance(_transform.position, resource.Position);

                    if (distance < minDistance)
                        closestResource = resource;
                }
            }

            return closestResource;
        }

        private void CleanupFoundResources()
        {
            for (int i = 0; i < _foundResources.Length; i++)
            {
                if (_foundResources[i] == null)
                    break;

                _foundResources[i] = null;
            }
        }
    }
}