using UnityEngine;
using Pooling;

public class UnloadedResourceParticleSystem : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        PoolStorage.PutToPool(nameof(UnloadedResourceParticleSystem), this);
    }
}