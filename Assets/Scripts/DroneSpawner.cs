using UnityEngine;
using Pooling;

namespace Drones
{
    public class DroneSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _blueBaseTransform;
        [SerializeField] private Transform _redBaseTransform;

        [SerializeField] private Drone _blueFractionDronePrefab;
        [SerializeField] private Drone _redFractionDronePrefab;

        [SerializeField] private Transform[] _blueFractionSpawnpoints;
        [SerializeField] private Transform[] _redFractionSpawnpoints;

        public string BlueFractionDroneKey => _blueFractionDronePrefab.name;
        public string RedFractionDroneKey => _redFractionDronePrefab.name;

        public Drone SpawnAtRandomPosition(Fraction fraction)
        {
            int index = -1;
            Vector3 position = Vector3.zero;
            Drone prefab = null;
            Transform baseTransform = null;

            if (fraction == Fraction.Blue)
            {
                index = Random.Range(0, _blueFractionSpawnpoints.Length);
                position = _blueFractionSpawnpoints[index].position;
                prefab = _blueFractionDronePrefab;
                baseTransform = _blueBaseTransform;
            }
            else if (fraction == Fraction.Red)
            {
                index = Random.Range(0, _redFractionSpawnpoints.Length);
                position = _redFractionSpawnpoints[index].position;
                prefab = _redFractionDronePrefab;
                baseTransform = _redBaseTransform;
            }

            Drone drone = PoolStorage.GetFromPool(prefab.name, prefab, position, Quaternion.identity);
            drone.Init(baseTransform);

            return drone;
        }
    }
}