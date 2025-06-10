using UnityEngine;
using Pooling;

namespace Drones
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private int _targetFrameRate = DefaultTargetFrameRate;

        private const int DefaultTargetFrameRate = 60;

        [SerializeField] private DronePopulationRegulator _dronePopulationRegulator;

        private void Awake()
        {
            Application.targetFrameRate = _targetFrameRate;

            PoolStorage.Init();

            _dronePopulationRegulator.Init();
        }
    }
}