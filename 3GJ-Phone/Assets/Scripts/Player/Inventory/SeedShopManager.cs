using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SeedShopManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform itemListParent;
    [SerializeField] private GameObject shopItemPrefab;

    private List<ItemData> allSeeds = new List<ItemData>();

    private void Start()
    {
        Debug.Log("SeedShopManager Start called");
        LoadAllSeeds();
        PopulateShop();
    }

    private void LoadAllSeeds()
    {
        ItemData[] seeds = Resources.LoadAll<ItemData>("Seeds");
        allSeeds = new List<ItemData>(seeds);
        Debug.Log($"Loaded seeds count: {allSeeds.Count}");
        foreach (var seed in allSeeds)
        {
            Debug.Log($"Loaded seed: {seed.itemName}");
        }
    }

    private void PopulateShop()
    {
        Debug.Log("PopulateShop called");
        foreach (Transform child in itemListParent)
            Destroy(child.gameObject);

        foreach (var seed in allSeeds)
        {
            Debug.Log($"Adding seed to shop: {seed.itemName}");
            GameObject go = Instantiate(shopItemPrefab, itemListParent);
            ShopItemUI ui = go.GetComponent<ShopItemUI>();
            if (ui == null)
            {
                Debug.LogError("ShopItemUI component missing on prefab!");
            }
            ui.Setup(seed, OnBuySeed);
        }
    }

    private void OnBuySeed(ItemData seed)
    {
        Debug.Log($"Attempting to buy seed: {seed.itemName} for {seed.buyCost}");
        if (PlayerWallet.instance.Spend(seed.buyCost))
        {
            Debug.Log($"Bought seed: {seed.itemName}");
            InventoryManager.instance.AddSeed(seed, 1);
        }
        else
        {
            Debug.Log("Not enough currency!");
        }
    }
    
    public void ToggleShop()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
    }
}
