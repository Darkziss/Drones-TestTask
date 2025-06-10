using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Drones
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Drone : MonoBehaviour
    {
        [SerializeField] private LayerMask _resourceLayer;

        private Transform _transform;

        private NavMeshAgent _navMeshAgent;

        private bool _wasFoundResource;

        private Coroutine _searchCoroutine;

        private readonly Collider[] _foundResources = new Collider[Capacity];

        private readonly WaitForSeconds _searchDelay = new(SearchDelay);

        private bool IsSearchingForResource => _searchCoroutine != null;

        private const int Capacity = 10;

        private const float SearchDelay = 0.1f;

        private const float CheckRadius = 30f;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;

            if (_navMeshAgent == null)
                _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            StartSearchingForResource();
        }

        private void StartSearchingForResource()
        {
            if (_wasFoundResource)
                throw new InvalidOperationException(nameof(IsSearchingForResource));

            _wasFoundResource = true;

            _searchCoroutine = StartCoroutine(TryFoundResource());
        }

        private void StopSearchingForResource()
        {
            if (!_wasFoundResource)
                throw new InvalidOperationException(nameof(IsSearchingForResource));

            _wasFoundResource = false;

            if (IsSearchingForResource)
                StopCoroutine(_searchCoroutine);
        }

        private IEnumerator TryFoundResource()
        {
            while (true)
            {
                int resourcesCount = Physics.OverlapSphereNonAlloc(_transform.position, CheckRadius, 
                    _foundResources, _resourceLayer);

                if (resourcesCount > 0)
                {
                    Collider collider = GetClosestResource();

                    Vector3 destinationPosition = collider.transform.position;

                    _wasFoundResource = true;
                    _navMeshAgent.SetDestination(destinationPosition);

                    StopSearchingForResource();
                }

                yield return _searchDelay;
            }
        }

        private Collider GetClosestResource()
        {
            float minDistance = float.MaxValue;
            int index = -1;

            for (int i = 0; i < _foundResources.Length; i++)
            {
                if (_foundResources[i] == null)
                    break;
                
                float distance = Vector3.Distance(_transform.position, 
                    _foundResources[i].transform.position);

                if (distance < minDistance)
                    index = i;
            }

            return _foundResources[index];
        }
    }
}