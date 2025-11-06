using UnityEngine;

/// <summary>
/// Centralized VFX manager for spawning particle effects.
/// Use this to spawn VFX with optional sound effects.
/// </summary>
public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    [Header("VFX Prefabs")]
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private GameObject muzzleFlashVFX;
    [SerializeField] private GameObject meleeSwingVFX;
    [SerializeField] private GameObject plantVFX;
    [SerializeField] private GameObject harvestVFX;
    [SerializeField] private GameObject waterVFX;
    [SerializeField] private GameObject explosionVFX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void SpawnVFX(string vfxName, Vector3 position, Quaternion rotation = default)
    {
        if (Instance == null) return;

        GameObject vfxPrefab = Instance.GetVFXPrefab(vfxName);
        
        if (vfxPrefab != null)
        {
            if (rotation == default)
                rotation = Quaternion.identity;
                
            Instantiate(vfxPrefab, position, rotation);
        }
    }

    private GameObject GetVFXPrefab(string vfxName)
    {
        return vfxName.ToLower() switch
        {
            "hit" => hitVFX,
            "death" => deathVFX,
            "muzzleflash" => muzzleFlashVFX,
            "meleeswing" => meleeSwingVFX,
            "plant" => plantVFX,
            "harvest" => harvestVFX,
            "water" => waterVFX,
            "explosion" => explosionVFX,
            _ => null
        };
    }
}
