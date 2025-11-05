using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "ItemData", menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;

    public enum ItemType { Seed, Crop, Weapon }
    [Header("Item Type")]
    public ItemType itemType = ItemType.Seed;

    [Header("Plant Properties (Seed/Crop only)")]
    public int buyCost = 50;
    public int sellValue = 25;
    [Range(1, 2)] public int growthTime = 1;

    [Header("Phase Sprites (Seed/Crop only)")]
    public Sprite plantedPhaseSprite;
    public Sprite growingPhaseSprite;
    public Sprite grownPhaseSprite;

    public enum SeedType { Normal, Turret }
    [Header("Seed Type (Seed only)")]
    public SeedType seedType = SeedType.Normal;

    [Header("Turret Properties (Turret Seed only)")]
    public GameObject turretPrefab;
}
