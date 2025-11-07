using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereGizmo))]
public class EnemySpawning : MonoBehaviour
{
    [Header("Enemy Prefabs")] [SerializeField]
    private GhostAI enemyPrefab;

    [SerializeField] private GhostAI fastEnemyPrefab;
    [SerializeField] private GhostAI slowEnemyPrefab;

    [Header("Spawn Weights (higher = more common)")] [SerializeField]
    private float normalEnemyWeight = 50f; // 50% chance

    [SerializeField] private float fastEnemyWeight = 30f; // 30% chance
    [SerializeField] private float slowEnemyWeight = 20f; // 20% chance

    [Header("Spawn Settings")] [SerializeField]
    private Transform spawnPoint;

    [SerializeField] private float minSpawnInterval = 2f;
    [SerializeField] private float maxSpawnInterval = 5f;

    private float spawnTimer;
    private bool canSpawn = false;
    private List<GhostAI> spawnedEnemies = new List<GhostAI>();

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        spawnTimer = UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval);
        canSpawn = GameManager.instance.state == GameManager.GameState.Night;
    }

    private void Update()
    {
        if (!canSpawn) return;

        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }

        if (spawnTimer <= 0)
        {
            SpawnRandomEnemy();
            spawnTimer = UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private void SpawnRandomEnemy()
    {
        List<(GhostAI prefab, float weight)> weightedEnemies = new List<(GhostAI, float)>();

        if (enemyPrefab != null && normalEnemyWeight > 0)
            weightedEnemies.Add((enemyPrefab, normalEnemyWeight));
        if (fastEnemyPrefab != null && fastEnemyWeight > 0)
            weightedEnemies.Add((fastEnemyPrefab, fastEnemyWeight));
        if (slowEnemyPrefab != null && slowEnemyWeight > 0)
            weightedEnemies.Add((slowEnemyPrefab, slowEnemyWeight));

        // If no enemies are assigned, log error and return
        if (weightedEnemies.Count == 0)
        {
            Debug.LogError("EnemySpawning: No enemy prefabs assigned or all weights are 0!");
            return;
        }

        // Calculate total weight
        float totalWeight = 0f;
        foreach (var enemy in weightedEnemies)
        {
            totalWeight += enemy.weight;
        }

        // Pick a random value between 0 and totalWeight
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        // Select enemy based on weight
        GhostAI selectedEnemy = null;
        float cumulativeWeight = 0f;

        foreach (var enemy in weightedEnemies)
        {
            cumulativeWeight += enemy.weight;
            if (randomValue <= cumulativeWeight)
            {
                selectedEnemy = enemy.prefab;
                break;
            }
        }

        // Fallback to first enemy if something went wrong
        if (selectedEnemy == null)
        {
            selectedEnemy = weightedEnemies[0].prefab;
        }

        // Spawn the selected enemy 
        GhostAI spawnedEnemy = Instantiate(selectedEnemy, spawnPoint.position, spawnPoint.rotation, transform);
        spawnedEnemies.Add(spawnedEnemy);

        Debug.Log($"Spawned enemy: {selectedEnemy.name}");
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        canSpawn = newState == GameManager.GameState.Night;
        if (newState == GameManager.GameState.Day)
        {
            foreach (var enemy in spawnedEnemies)
            {
                if (enemy != null)
                    Destroy(enemy.gameObject);
            }

            spawnedEnemies.Clear();
        }
    }
}