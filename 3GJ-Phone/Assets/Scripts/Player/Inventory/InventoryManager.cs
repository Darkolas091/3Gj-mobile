using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SeedInventoryEntry
{
    public ItemData seedData;
    public int quantity;
    public SeedInventoryEntry(ItemData data, int qty)
    {
        seedData = data;
        quantity = qty;
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Seeds (with quantity)")]
    public List<SeedInventoryEntry> seeds = new List<SeedInventoryEntry>();
    [Header("Unlocked Weapons")]
    public List<ItemData> unlockedWeapons = new List<ItemData>();
    [Header("Crops (count only, for shop)")]
    private Dictionary<ItemData, int> cropCounts = new Dictionary<ItemData, int>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Add seed (with quantity)
    public void AddSeed(ItemData seed, int amount = 1)
    {
        foreach (var entry in seeds)
        {
            if (entry.seedData == seed)
            {
                entry.quantity += amount;
                return;
            }
        }
        seeds.Add(new SeedInventoryEntry(seed, amount));
    }

    // Remove seed (with quantity)
    public bool RemoveSeed(ItemData seed, int amount = 1)
    {
        for (int i = 0; i < seeds.Count; i++)
        {
            if (seeds[i].seedData == seed)
            {
                if (seeds[i].quantity < amount) return false;
                seeds[i].quantity -= amount;
                if (seeds[i].quantity <= 0) seeds.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public int GetSeedQuantity(ItemData seed)
    {
        foreach (var entry in seeds)
        {
            if (entry.seedData == seed)
                return entry.quantity;
        }
        return 0;
    }

    // Add weapon (unlocked)
    public void UnlockWeapon(ItemData weapon)
    {
        if (!unlockedWeapons.Contains(weapon))
            unlockedWeapons.Add(weapon);
    }

    public bool IsWeaponUnlocked(ItemData weapon)
    {
        return unlockedWeapons.Contains(weapon);
    }

    // Get all seeds
    public List<ItemData> GetAllSeeds()
    {
        List<ItemData> result = new List<ItemData>();
        foreach (var entry in seeds)
        {
            if (entry.quantity > 0)
                result.Add(entry.seedData);
        }
        return result;
    }

    // Get all unlocked weapons
    public List<ItemData> GetAllUnlockedWeapons()
    {
        return new List<ItemData>(unlockedWeapons);
    }

    // Add crop (after harvesting)
    public void AddCrop(ItemData crop, int amount = 1)
    {
        if (cropCounts.ContainsKey(crop))
            cropCounts[crop] += amount;
        else
            cropCounts[crop] = amount;
    }

    // Get crop count (for shop menu)
    public int GetCropCount(ItemData crop)
    {
        if (cropCounts.ContainsKey(crop))
            return cropCounts[crop];
        else
            return 0;
    }
}
