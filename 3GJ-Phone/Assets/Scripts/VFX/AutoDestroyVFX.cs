using UnityEngine;

/// <summary>
/// Simple VFX spawner with auto-destroy.
/// Attach this to any particle effect prefab to auto-destroy it when done.
/// </summary>
public class AutoDestroyVFX : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private bool useParticleSystemDuration = true;

    private void Start()
    {
        if (useParticleSystemDuration)
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            if (ps != null)
            {
                lifetime = ps.main.duration + ps.main.startLifetime.constantMax;
            }
        }
        
        Destroy(gameObject, lifetime);
    }
}