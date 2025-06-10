using UnityEngine;

namespace Drones.Utils
{
    public static class DroneUtils
    {
        public static bool IsPassedMinDistanceToPositionWithExcludedY(Vector3 position, Vector3 targetPosition, float minDistance)
        {
            position.y = 0f;
            targetPosition.y = 0f;

            float distance = Vector3.Distance(position, targetPosition);

            return distance < minDistance;
        }
    }
}