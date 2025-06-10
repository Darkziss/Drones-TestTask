using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Pooling;
using UnityRandom = UnityEngine.Random;

namespace Drones
{
    public class ResourceSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _arenaTransform;

        [SerializeField] private Resource _resourcePrefab;

        [SerializeField] private LayerMask _obstacleLayer;

        private Coroutine _spawnCoroutine;
        private readonly WaitForSeconds _spawnDelay = new(SpawnDelay);

        private bool IsSpawning => _spawnCoroutine != null;

        private const float SpawnDelay = 0.5f;

        private const string ResourceName = "Resource";

        private const float MaxDistance = 10f;

        private const float YPosition = 1f;

        private const float CheckRadius = 0.5f;

        public void StartSpawning()
        {
            if (IsSpawning)
                return;

            _spawnCoroutine = StartCoroutine(SpawnWithDelay());
        }

        public void StopSpawning()
        {
            if (!IsSpawning)
                return;

            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        private IEnumerator SpawnWithDelay()
        {
            while (true)
            {
                yield return _spawnDelay;

                Vector3 position = GetRandomPosition();

                PoolStorage.GetFromPool(ResourceName, _resourcePrefab, position, Quaternion.identity);
            }
        }

        private Vector3 GetRandomPosition()
        {
            while (true)
            {
                float xArenaScale = _arenaTransform.localScale.x / 2f;
                float zArenaScale = _arenaTransform.localScale.z / 2f;

                Vector3 randomPosition = new()
                {
                    x = UnityRandom.Range(-xArenaScale, xArenaScale),
                    y = YPosition,
                    z = UnityRandom.Range(-zArenaScale, zArenaScale)
                };

                if (!NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, MaxDistance, NavMesh.AllAreas))
                    throw new Exception();

                if (!Physics.CheckSphere(randomPosition, CheckRadius, _obstacleLayer))
                {
                    Vector3 sampledPosition = new(hit.position.x, YPosition, hit.position.z);

                    return sampledPosition;
                }
                    
            }
        }
    }
}