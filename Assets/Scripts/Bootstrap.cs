using UnityEngine;

namespace Drones
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private int _targetFrameRate = DefaultTargetFrameRate;

        private const int DefaultTargetFrameRate = 60;

        private void Awake()
        {
            Application.targetFrameRate = _targetFrameRate;
        }
    }
}