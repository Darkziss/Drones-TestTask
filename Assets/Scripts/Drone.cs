using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Drones.Utils;

namespace Drones
{
    [RequireComponent(typeof(NavMeshAgent), typeof(ResourceFinder))]
    public class Drone : MonoBehaviour
    {
        [SerializeField] private Transform _baseTransform;

        [SerializeField] private LayerMask _resourceLayer;

        private Transform _transform;
        private NavMeshAgent _navMeshAgent;
        private ResourceFinder _resourceFinder;

        private DroneState _state;

        private Resource _targetResource;

        private readonly WaitForSeconds _searchDelay = new(SearchDelay);
        private readonly WaitForSeconds _collectDelay = new(CollectDelay);

        private readonly Collider[] _foundResources = new Collider[Capacity];

        private const int Capacity = 15;

        private const float SearchDelay = 0.1f;
        private const float CollectDelay = 2f;

        private const float CheckRadius = 100f;

        private const float MinCollectingDistance = 0.01f;
        private const float MinUnloadingDistance = 2f;

        public event Action ResourceUnloaded;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;

            if (_navMeshAgent == null)
                _navMeshAgent = GetComponent<NavMeshAgent>();

            if (_resourceFinder == null)
                _resourceFinder = GetComponent<ResourceFinder>();
        }

        private void Start()
        {
            SetState(DroneState.Searching);
        }

        private void Update()
        {
            if (_state == DroneState.Found 
                && DroneUtils.IsPassedMinDistanceToPositionWithExcludedY(_transform.position, _targetResource.Position, MinCollectingDistance))
            {
                SetState(DroneState.Collecting);
            }
            else if (_state == DroneState.Delivering 
                && DroneUtils.IsPassedMinDistanceToPositionWithExcludedY(_transform.position, _baseTransform.position, MinUnloadingDistance))
            {
                SetState(DroneState.Unloading);
            }
        }

        private void SetState(DroneState requestedState)
        {
            if (requestedState == _state)
                return;

            _state = requestedState;

            switch (_state)
            {
                case DroneState.Searching:
                    _resourceFinder.StartSearch(SetTargetResource);
                    break;
                case DroneState.Found:
                    _targetResource.Reserve();
                    _navMeshAgent.SetDestination(_targetResource.Position);
                    break;
                case DroneState.Collecting:
                    StartCollectingResource();
                    break;
                case DroneState.Delivering:
                    StartDeliveringResourceToBase();
                    break;
                case DroneState.Unloading:
                    UnloadResource();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(requestedState));
            }
        }

        private void SetTargetResource(Resource resource)
        {
            _targetResource = resource;

            SetState(DroneState.Found);
        }

        private void StartCollectingResource()
        {
            _navMeshAgent.isStopped = true;

            StartCoroutine(CollectResourceWithDelay());
        }

        private IEnumerator CollectResourceWithDelay()
        {
            yield return _collectDelay;

            _targetResource.Collect();
            SetState(DroneState.Delivering);
        }

        private void StartDeliveringResourceToBase()
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_baseTransform.position);
        }

        private void UnloadResource()
        {
            _targetResource.Release();
            _targetResource = null;

            ResourceUnloaded?.Invoke();

            SetState(DroneState.Searching);
        }
    }
}