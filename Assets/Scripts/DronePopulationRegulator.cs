using System.Collections.Generic;
using UnityEngine;
using Pooling;

namespace Drones
{
    public class DronePopulationRegulator : MonoBehaviour
    {
        [SerializeField] private DroneSpawner _droneSpawner;

        private int _blueFractionDroneCount;
        private int _redFractionDroneCount;

        private readonly List<Drone> _blueFractionDrones = new();
        private readonly List<Drone> _redFractionDrones = new();

        public void Init()
        {
            SetBlueFractionDroneCount(1f);
            SetRedFractionDroneCount(1f);
        }

        public void SetBlueFractionDroneCount(float requestedCountF)
        {
            int requestedCount = (int)requestedCountF;

            if (requestedCount > _blueFractionDroneCount)
            {
                int addCount = requestedCount - _blueFractionDroneCount;
                SpawnDrones(addCount, Fraction.Blue);
            }
            else if (requestedCount < _blueFractionDroneCount)
            {
                int deleteCount = _blueFractionDroneCount - requestedCount;
                DeleteDrones(deleteCount, Fraction.Blue);
            }

            _blueFractionDroneCount = requestedCount;
        }

        public void SetRedFractionDroneCount(float requestedCountF)
        {
            int requestedCount = (int)requestedCountF;

            if (requestedCount > _redFractionDroneCount)
            {
                int addCount = requestedCount - _redFractionDroneCount;
                SpawnDrones(addCount, Fraction.Red);
            }
            else if (requestedCount < _redFractionDroneCount)
            {
                int deleteCount = _redFractionDroneCount - requestedCount;
                DeleteDrones(deleteCount, Fraction.Red);
            }

            _redFractionDroneCount = requestedCount;
        }

        private void SpawnDrones(int count, Fraction fraction)
        {
            List<Drone> list = GetListByFraction(fraction);

            for (int i = 0; i < count; i++)
            {
                Drone drone = _droneSpawner.SpawnAtRandomPosition(fraction);
                list.Add(drone);
            }
        }

        private void DeleteDrones(int count, Fraction fraction)
        {
            List<Drone> list = GetListByFraction(fraction);

            for (int i = 0; i < count; i++)
            {
                string name = GetNameByFraction(fraction);
                PoolStorage.PutToPool(name, list[i]);
            }

            list.RemoveRange(0, count);
        }

        private string GetNameByFraction(Fraction fraction)
        {
            return fraction == Fraction.Blue ? _droneSpawner.BlueFractionDroneKey : _droneSpawner.RedFractionDroneKey;
        }

        private List<Drone> GetListByFraction(Fraction fraction)
        {
            return fraction == Fraction.Blue ? _blueFractionDrones : _redFractionDrones;
        }
    }
}