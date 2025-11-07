using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameGrid : MonoBehaviour
{
    [Header("Grid 1")]
    [SerializeField] private int columnLength = 5, rowLength = 5;
    [SerializeField] private float x_space = 1f, z_space = 1f;
    [SerializeField] private Vector3 gridStartPosition = new Vector3(-8.42f, 0, 11.9f);

    [Header("Grid 2")]
    [SerializeField] private int columnLength2 = 3, rowLength2 = 3;
    [SerializeField] private float x_space2 = 1f, z_space2 = 1f;
    [SerializeField] private Vector3 gridStartPosition2 = new Vector3(5f, 0, 11.9f);

    [Header("Grid 3 & 4 - Premium Crops")]
    [SerializeField] private int columnLength3 = 2, rowLength3 = 2;
    [SerializeField] private float x_space3 = 1f, z_space3 = 1f;
    [SerializeField] private Vector3 gridStartPosition3 = new Vector3(-8.42f, 0, 5f);
    
    [SerializeField] private int columnLength4 = 2, rowLength4 = 2;
    [SerializeField] private float x_space4 = 1f, z_space4 = 1f;
    [SerializeField] private Vector3 gridStartPosition4 = new Vector3(5f, 0, 5f);

    [Header("Prefabs")]
    [SerializeField] private GameObject grass;
    [SerializeField] private GameObject field; // Regular crop
    [SerializeField] private GameObject premiumField; // Premium crop for grids 3 & 4
    [SerializeField] private GameObject turretPrefab; // Turret that premium crops turn into

    [Header("UI")]
    [SerializeField] private Button actionButton;
    [SerializeField] private TMP_Text buttonText;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactionDistance = 3f;

    [Header("Water System")]
    [SerializeField] private int waterAmount = 100;
    [SerializeField] private int waterCostPerUse = 10;
    [SerializeField] private TMP_Text waterText; // Optional: to display water amount
    [SerializeField] private Color wateredColor = new Color(0.7f, 0.9f, 1f, 1f); // Light blue tint for watered plants
    [SerializeField] private Color normalColor = Color.white; // Normal plant color

    [Header("Harvest System")]
    [SerializeField] private int plantCost = 10; // Cost to plant a regular crop
    [SerializeField] private int harvestMoneyReward = 50;
    [SerializeField] private int premiumPlantCost = 100; // Cost to plant premium crop (grids 3 & 4)
    [SerializeField] private int premiumHarvestReward = 300; // Premium crop harvest reward
    [SerializeField] private int growthStagesNeeded = 2; // After 2 growth stages, can harvest
    [SerializeField] private TMP_Text moneyText; // Optional: to display money

    private GameObject selectedTile;
    private GameManager.GameState lastGameState;
    private HashSet<Vector3> plantedPositions = new HashSet<Vector3>();
    private Dictionary<Vector3, GameObject> plantObjects = new Dictionary<Vector3, GameObject>();
    private Dictionary<Vector3, bool> wateredToday = new Dictionary<Vector3, bool>(); // Track if plant was watered today
    private Dictionary<Vector3, int> plantGrowthStage = new Dictionary<Vector3, int>(); // Track growth stages
    private Dictionary<GameObject, Vector3> plantToOriginalPos = new Dictionary<GameObject, Vector3>(); // Track original position of each plant
    private HashSet<Vector3> premiumTilePositions = new HashSet<Vector3>(); // Track which tiles are premium (grids 3 & 4)
    private Dictionary<Vector3, bool> isPremiumPlant = new Dictionary<Vector3, bool>(); // Track which plants are premium
    private Dictionary<Vector3, GameObject> turretObjects = new Dictionary<Vector3, GameObject>(); // Track turrets on plots

    void Start()
    {
        if (actionButton != null)
        {
            actionButton.gameObject.SetActive(false);
            actionButton.onClick.AddListener(OnActionButtonClicked);
        }

        // Create Grid 1
        CreateGrid(columnLength, rowLength, x_space, z_space, gridStartPosition);
        
        // Create Grid 2
        CreateGrid(columnLength2, rowLength2, x_space2, z_space2, gridStartPosition2);

        // Create Grid 3 & 4 (Premium Grids)
        CreatePremiumGrid(columnLength3, rowLength3, x_space3, z_space3, gridStartPosition3);
        CreatePremiumGrid(columnLength4, rowLength4, x_space4, z_space4, gridStartPosition4);

        UpdateWaterDisplay();
        UpdateMoneyDisplay();
        
        // Subscribe to game state changes to detect new day
        GameManager.OnGameStateChanged += OnGameStateChanged;
        if (GameManager.instance != null)
        {
            lastGameState = GameManager.instance.state;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe when destroyed
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        // Detect when transitioning from Night to Day (new day starts)
        if (lastGameState == GameManager.GameState.Night && newState == GameManager.GameState.Day)
        {
            OnNewDay();
        }
        lastGameState = newState;
    }

    private void CreateGrid(int cols, int rows, float xSpace, float zSpace, Vector3 startPos)
    {
        if (grass == null) return;
        
        for (int i = 0; i < cols * rows; i++)
        {
            float xPos = startPos.x + xSpace * (i % cols);
            float zPos = startPos.z + zSpace * (i / cols);
            GameObject tile = Instantiate(grass, new Vector3(xPos, startPos.y, zPos), Quaternion.identity);
            tile.tag = "grid";
        }
    }

    private void CreatePremiumGrid(int cols, int rows, float xSpace, float zSpace, Vector3 startPos)
    {
        if (grass == null) return;
        
        for (int i = 0; i < cols * rows; i++)
        {
            float xPos = startPos.x + xSpace * (i % cols);
            float zPos = startPos.z + zSpace * (i / cols);
            Vector3 tilePos = new Vector3(xPos, startPos.y, zPos);
            GameObject tile = Instantiate(grass, tilePos, Quaternion.identity);
            tile.tag = "grid";
            
            // Mark this tile as premium
            premiumTilePositions.Add(RoundPosition(tilePos));
        }
    }

    private void Update()
    {
        if (player != null)
        {
            CheckNearestTile();
        }
    }

    private void CheckNearestTile()
    {
        // Check both grid tiles AND planted crops
        GameObject[] allTiles = GameObject.FindGameObjectsWithTag("grid");
        GameObject[] allPlants = GameObject.FindGameObjectsWithTag("Plant");
        
        GameObject nearestObject = null;
        float nearestDistance = interactionDistance;
        bool isPlantObject = false;

        // Check grid tiles
        foreach (GameObject tile in allTiles)
        {
            float distance = Vector3.Distance(player.position, tile.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestObject = tile;
                isPlantObject = false;
            }
        }

        // Check plants
        foreach (GameObject plant in allPlants)
        {
            float distance = Vector3.Distance(player.position, plant.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestObject = plant;
                isPlantObject = true;
            }
        }

        if (nearestObject != selectedTile)
        {
            selectedTile = nearestObject;
            
            if (selectedTile != null)
            {
                Vector3 pos;
                
                // If it's a plant object, use its original position
                if (isPlantObject && plantToOriginalPos.ContainsKey(selectedTile))
                {
                    pos = plantToOriginalPos[selectedTile];
                }
                else
                {
                    pos = RoundPosition(selectedTile.transform.position);
                }
                
                Debug.Log($"Selected {(isPlantObject ? "PLANT" : "TILE")} at {pos}");
                
                // Check if there's a turret at this position
                if (turretObjects.ContainsKey(pos))
                {
                    ShowButton("Turret Active");
                }
                // If it's a plant or the position has a plant
                else if (isPlantObject || plantedPositions.Contains(pos))
                {
                    // Check if plant is ready to harvest
                    int currentStage = plantGrowthStage.ContainsKey(pos) ? plantGrowthStage[pos] : 0;
                    
                    Debug.Log($"Plant growth stage: {currentStage}/{growthStagesNeeded}");
                    
                    bool isPremium = isPremiumPlant.ContainsKey(pos) && isPremiumPlant[pos];
                    
                    if (currentStage >= growthStagesNeeded)
                    {
                        // Premium crops don't show harvest button - they auto-convert to turrets
                        if (isPremium)
                        {
                            ShowButton("Growing into Turret...");
                        }
                        else
                        {
                            // Regular crops can be harvested
                            ShowButton($"Harvest (+${harvestMoneyReward})");
                        }
                    }
                    else if (wateredToday.ContainsKey(pos) && wateredToday[pos])
                    {
                        ShowButton("Already Watered");
                    }
                    else
                    {
                        ShowButton($"Water (-{waterCostPerUse})");
                    }
                }
                else
                {
                    // Show different plant cost based on if it's a premium tile
                    bool isPremiumTile = premiumTilePositions.Contains(pos);
                    int cost = isPremiumTile ? premiumPlantCost : plantCost;
                    ShowButton($"Plant (-${cost})");
                }
            }
            else
            {
                HideButton();
            }
        }
    }

    private void ShowButton(string text)
    {
        if (actionButton != null)
        {
            actionButton.gameObject.SetActive(true);
            if (buttonText != null)
            {
                buttonText.text = text;
            }
        }
    }

    private void HideButton()
    {
        if (actionButton != null)
        {
            actionButton.gameObject.SetActive(false);
        }
    }

    private void OnActionButtonClicked()
    {
        Debug.Log("ACTION BUTTON CLICKED!");
        
        if (selectedTile == null)
        {
            Debug.Log("Selected tile is null!");
            return;
        }

        Vector3 pos;
        
        // If selected tile is a plant, get its original position
        if (plantToOriginalPos.ContainsKey(selectedTile))
        {
            pos = plantToOriginalPos[selectedTile];
        }
        else
        {
            pos = RoundPosition(selectedTile.transform.position);
        }
        
        Debug.Log($"Tile position: {pos}, Is planted: {plantedPositions.Contains(pos)}");

        if (plantedPositions.Contains(pos))
        {
            int currentStage = plantGrowthStage.ContainsKey(pos) ? plantGrowthStage[pos] : 0;
            
            Debug.Log($"Current growth stage: {currentStage}/{growthStagesNeeded}");
            
            // Check if plant is ready to harvest
            if (currentStage >= growthStagesNeeded)
            {
                HarvestPlant(pos);
                return;
            }
            
            // Check if already watered today
            if (wateredToday.ContainsKey(pos) && wateredToday[pos])
            {
                Debug.Log("Plant already watered today!");
                return;
            }

            // Check if we have enough water
            if (waterAmount < waterCostPerUse)
            {
                Debug.Log("Not enough water!");
                return;
            }

            // Water the plant - consume water and mark as watered
            if (plantObjects.ContainsKey(pos))
            {
                waterAmount -= waterCostPerUse;
                UpdateWaterDisplay();
                
                wateredToday[pos] = true; // Mark as watered today
                
                // Initialize growth stage if needed
                if (!plantGrowthStage.ContainsKey(pos))
                {
                    plantGrowthStage[pos] = 0;
                }
                
                // Change plant color to show it's been watered
                GameObject plant = plantObjects[pos];
                if (plant != null)
                {
                    SpriteRenderer spriteRenderer = plant.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = wateredColor;
                    }
                    
                    // Also check children for sprite renderers
                    SpriteRenderer[] childRenderers = plant.GetComponentsInChildren<SpriteRenderer>();
                    foreach (var renderer in childRenderers)
                    {
                        renderer.color = wateredColor;
                    }
                }
                
                Debug.Log($"PLANT WATERED! Color changed to show watered state. It will grow at the start of the next day. Water remaining: {waterAmount}");
            }
            else
            {
                Debug.Log("Plant object not found in dictionary!");
            }
        }
        else
        {
            // Check if this is a premium tile
            bool isPremiumTile = premiumTilePositions.Contains(pos);
            int cost = isPremiumTile ? premiumPlantCost : plantCost;
            GameObject cropPrefab = isPremiumTile ? premiumField : field;
            
            // Plant a new crop - check if player has enough money
            if (PlayerWallet.instance != null && PlayerWallet.instance.currency < cost)
            {
                Debug.Log($"Not enough money to plant! Need ${cost}, have ${PlayerWallet.instance.currency}");
                return;
            }
            
            if (cropPrefab != null)
            {
                // Deduct money
                if (PlayerWallet.instance != null)
                {
                    PlayerWallet.instance.Spend(cost);
                    UpdateMoneyDisplay();
                }
                
                GameObject plant = Instantiate(cropPrefab, selectedTile.transform.position, Quaternion.identity);
                plant.tag = "Plant";
                plantedPositions.Add(pos);
                plantObjects[pos] = plant;
                plantToOriginalPos[plant] = pos; // Track original position
                plantGrowthStage[pos] = 0; // Start at growth stage 0
                wateredToday[pos] = false; // Not watered yet
                isPremiumPlant[pos] = isPremiumTile; // Track if this is a premium plant
                
                Destroy(selectedTile);
                Debug.Log($"Planted {(isPremiumTile ? "PREMIUM" : "regular")} crop! Money remaining: ${PlayerWallet.instance?.currency ?? 0}");
            }
            else
            {
                Debug.Log($"{(isPremiumTile ? "Premium field" : "Field")} prefab is null!");
            }
        }
    }

    private Vector3 RoundPosition(Vector3 pos)
    {
        return new Vector3(
            Mathf.Round(pos.x * 10f) / 10f,
            Mathf.Round(pos.y * 10f) / 10f,
            Mathf.Round(pos.z * 10f) / 10f
        );
    }

    private void UpdateWaterDisplay()
    {
        if (waterText != null)
        {
            waterText.text = $"Water: {waterAmount}";
        }
    }

    private void UpdateMoneyDisplay()
    {
        if (moneyText != null && PlayerWallet.instance != null)
        {
            moneyText.text = $"Money: ${PlayerWallet.instance.currency}";
        }
    }

    private void HarvestPlant(Vector3 pos)
    {
        if (plantObjects.ContainsKey(pos))
        {
            GameObject plant = plantObjects[pos];
            
            // Check if this is a premium plant
            bool isPremium = isPremiumPlant.ContainsKey(pos) && isPremiumPlant[pos];
            int reward = isPremium ? premiumHarvestReward : harvestMoneyReward;
            
            // Add money to PlayerWallet
            if (PlayerWallet.instance != null)
            {
                PlayerWallet.instance.Add(reward);
                UpdateMoneyDisplay(); // Update the money display
                Debug.Log($"HARVESTED {(isPremium ? "PREMIUM" : "regular")} crop! Gained ${reward}. Total money: ${PlayerWallet.instance.currency}");
            }
            else
            {
                Debug.LogWarning("PlayerWallet instance not found!");
            }
            
            // Clean up dictionaries
            if (plant != null)
            {
                plantToOriginalPos.Remove(plant); // Remove from tracking
                Destroy(plant);
            }
            
            plantObjects.Remove(pos);
            plantedPositions.Remove(pos);
            wateredToday.Remove(pos);
            plantGrowthStage.Remove(pos);
            bool wasPremium = isPremiumPlant.ContainsKey(pos) && isPremiumPlant[pos];
            isPremiumPlant.Remove(pos);
            
            // Respawn grass tile at this position (at the original Y level)
            if (grass != null)
            {
                GameObject newTile = Instantiate(grass, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
                newTile.tag = "grid";
                
                // If it was a premium tile, re-add it to premium positions
                if (wasPremium)
                {
                    premiumTilePositions.Add(pos);
                }
            }
        }
    }

    // Call this method when a new day starts (hook this up to your day/night cycle)
    public void OnNewDay()
    {
        Debug.Log("New day started! Growing watered plants...");
        
        // Grow all plants that were watered yesterday
        List<Vector3> positionsToConvert = new List<Vector3>(); // Track premium crops that need to become turrets
        
        foreach (var kvp in wateredToday)
        {
            Vector3 pos = kvp.Key;
            bool wasWatered = kvp.Value;
            
            if (wasWatered && plantObjects.ContainsKey(pos))
            {
                GameObject plant = plantObjects[pos];
                if (plant != null)
                {
                    // Grow the plant and move it up by 50%
                    plant.transform.localScale *= 1.5f; // 50% growth
                    plant.transform.position += new Vector3(0, 0.1f, 0); // Move up by 0.1
                    plantGrowthStage[pos]++;
                    
                    // Reset color back to normal after growing
                    SpriteRenderer spriteRenderer = plant.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = normalColor;
                    }
                    
                    // Also reset children sprite renderers
                    SpriteRenderer[] childRenderers = plant.GetComponentsInChildren<SpriteRenderer>();
                    foreach (var renderer in childRenderers)
                    {
                        renderer.color = normalColor;
                    }
                    
                    Debug.Log($"Plant at {pos} grew 50% to stage {plantGrowthStage[pos]} and moved up!");
                    
                    // Check if this is a premium plant that's fully grown
                    bool isPremium = isPremiumPlant.ContainsKey(pos) && isPremiumPlant[pos];
                    if (isPremium && plantGrowthStage[pos] >= growthStagesNeeded)
                    {
                        positionsToConvert.Add(pos);
                    }
                }
            }
        }
        
        // Convert premium crops to turrets
        foreach (Vector3 pos in positionsToConvert)
        {
            ConvertToTurret(pos);
        }
        
        // Reset all watered flags for the new day
        var keys = new List<Vector3>(wateredToday.Keys);
        foreach (var key in keys)
        {
            wateredToday[key] = false;
        }
    }

    private void ConvertToTurret(Vector3 pos)
    {
        if (!plantObjects.ContainsKey(pos)) return;
        
        GameObject plant = plantObjects[pos];
        
        if (turretPrefab != null)
        {
            // Spawn turret at the plant's current position
            GameObject turret = Instantiate(turretPrefab, plant.transform.position, Quaternion.identity);
            turret.tag = "Turret";
            
            // Add a component to track the turret and its plot
            TurretPlotTracker tracker = turret.AddComponent<TurretPlotTracker>();
            tracker.plotPosition = pos;
            tracker.gridManager = this;
            
            // Track the turret
            turretObjects[pos] = turret;
            
            Debug.Log($"Premium crop at {pos} converted to TURRET!");
        }
        
        // Destroy the plant
        if (plant != null)
        {
            plantToOriginalPos.Remove(plant);
            Destroy(plant);
        }
        
        // Clean up plant tracking but keep the position occupied
        plantObjects.Remove(pos);
        plantedPositions.Remove(pos);
        wateredToday.Remove(pos);
        plantGrowthStage.Remove(pos);
        isPremiumPlant.Remove(pos);
    }

    // Called by TurretPlotTracker when turret is destroyed
    public void OnTurretDestroyed(Vector3 pos)
    {
        Debug.Log($"Turret at {pos} destroyed! Respawning grass tile...");
        
        // Remove turret tracking
        if (turretObjects.ContainsKey(pos))
        {
            turretObjects.Remove(pos);
        }
        
        // Respawn grass tile
        if (grass != null)
        {
            GameObject newTile = Instantiate(grass, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
            newTile.tag = "grid";
            
            // Re-add to premium positions if it was a premium tile
            if (premiumTilePositions.Contains(pos))
            {
                // Already in the set, no need to re-add
            }
            else
            {
                // Check if it should be premium based on the grid positions
                premiumTilePositions.Add(pos);
            }
        }
    }

    // Public method to add water (for pickups, shop, etc.)
    public void AddWater(int amount)
    {
        waterAmount += amount;
        UpdateWaterDisplay();
        Debug.Log($"Added {amount} water. Total: {waterAmount}");
    }

    // Public method to get current water amount
    public int GetWaterAmount()
    {
        return waterAmount;
    }
}
