using UnityEngine;
using System;

public class FarmlandTile : MonoBehaviour
{
    public enum GrowthPhase
    {
        Empty,
        Planted,
        Growing,
        Grown
    }

    [Header("Tile State")] [SerializeField]
    private GrowthPhase phase = GrowthPhase.Empty;

    [SerializeField] private ItemData plantedItem;
    [SerializeField] private bool isWatered = false;
    [SerializeField] private int daysSincePlanted = 0;

    [Header("Visuals")] [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite emptySprite;

    [Header("Turret Settings")]
    [SerializeField] private GameObject turretPrefab;
    private GameObject turretInstance;
    [SerializeField] private bool isPlotDisabled = false;

    public event Action<GrowthPhase> OnPhaseChanged;
    public event Action OnHarvested;

    public GrowthPhase Phase => phase;
    public ItemData PlantedItem => plantedItem;
    public bool IsWatered => isWatered;
    public int DaysSincePlanted => daysSincePlanted;

    public void PlantSeed(ItemData seed)
    {
        if (isPlotDisabled) return;
        plantedItem = seed;
        phase = GrowthPhase.Planted;
        daysSincePlanted = 0;
        isWatered = false;
        UpdateVisuals();
        OnPhaseChanged?.Invoke(phase);
    }

    public void WaterTile()
    {
        isWatered = true;
        // Future: Play SFX/VFX
    }

    public void AdvanceDay()
    {
        // Only advance if something is planted and phase is Planted or Growing
        if (plantedItem == null) return;
        if (phase != GrowthPhase.Planted && phase != GrowthPhase.Growing) return;

        if (isWatered)
        {
            daysSincePlanted++;
            isWatered = false;

            // 1-day seed: Planted -> Grown
            if (plantedItem.growthTime == 1)
            {
                phase = GrowthPhase.Grown;
            }
            // 2-day seed: Planted -> Growing -> Grown
            else if (plantedItem.growthTime == 2)
            {
                if (phase == GrowthPhase.Planted)
                {
                    phase = GrowthPhase.Growing;
                }
                else if (phase == GrowthPhase.Growing)
                {
                    phase = GrowthPhase.Grown;
                }
            }

            // If turret seed and now grown, spawn turret and disable plot
            if (phase == GrowthPhase.Grown && plantedItem.seedType == ItemData.SeedType.Turret)
            {
                SpawnTurret();
                isPlotDisabled = true;
            }

            UpdateVisuals();
            OnPhaseChanged?.Invoke(phase);
        }
    }

    private void SpawnTurret()
    {
        if (turretPrefab != null && turretInstance == null)
        {
            turretInstance = Instantiate(turretPrefab, transform.position, Quaternion.identity, transform);
            // var turretHealth = turretInstance.GetComponent<TurretHealth>();
            // if (turretHealth != null)
            // {
            //     turretHealth.OnTurretDestroyed += HandleTurretDestroyed;
            // }
        }
    }

    private void HandleTurretDestroyed()
    {
        isPlotDisabled = false;
        turretInstance = null;
        plantedItem = null;
        phase = GrowthPhase.Empty;
        daysSincePlanted = 0;
        isWatered = false;
        UpdateVisuals();
        OnPhaseChanged?.Invoke(phase);
    }

    public void HarvestCrop()
    {
        if (phase == GrowthPhase.Grown && (plantedItem == null || plantedItem.seedType != ItemData.SeedType.Turret))
        {
            plantedItem = null;
            phase = GrowthPhase.Empty;
            daysSincePlanted = 0;
            isWatered = false;
            UpdateVisuals();
            OnHarvested?.Invoke();
        }
    }

    public GrowthPhase GetStatus() => phase;

    private void UpdateVisuals()
    {
        if (spriteRenderer == null) return;
        switch (phase)
        {
            case GrowthPhase.Empty:
                spriteRenderer.sprite = emptySprite;
                break;
            case GrowthPhase.Planted:
                if (plantedItem != null && plantedItem.plantedPhaseSprite != null)
                    spriteRenderer.sprite = plantedItem.plantedPhaseSprite;
                else
                    spriteRenderer.sprite = emptySprite;
                break;
            case GrowthPhase.Growing:
                if (plantedItem != null && plantedItem.growingPhaseSprite != null)
                    spriteRenderer.sprite = plantedItem.growingPhaseSprite;
                else
                    spriteRenderer.sprite = emptySprite;
                break;
            case GrowthPhase.Grown:
                if (plantedItem != null && plantedItem.grownPhaseSprite != null)
                    spriteRenderer.sprite = plantedItem.grownPhaseSprite;
                else
                    spriteRenderer.sprite = emptySprite;
                break;
        }
        // Future: Add SFX/VFX hooks here
    }
}