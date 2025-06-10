using UnityEngine;
using Pooling;

namespace Drones
{
    [RequireComponent(typeof(Drone))]
    public class DroneParticleEmitter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _unloadedResourcePSPrefab;

        [SerializeField] private Transform _emitPoint;

        private Drone _drone;

        private void Awake()
        {
            if (_drone == null)
                _drone = GetComponent<Drone>();
        }

        private void OnEnable()
        {
            _drone.ResourceUnloaded += PlayUnloadedResourceEffect;
        }

        private void OnDisable()
        {
            _drone.ResourceUnloaded -= PlayUnloadedResourceEffect;
        }

        private void PlayUnloadedResourceEffect()
        {
            ParticleSystem particleSystem = PoolStorage.GetFromPool(nameof(UnloadedResourceParticleSystem),
                _unloadedResourcePSPrefab, _emitPoint.position, Quaternion.identity);

            particleSystem.Play();
        }
    }
}