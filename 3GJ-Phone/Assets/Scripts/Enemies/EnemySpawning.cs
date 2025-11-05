using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereGizmo))]
public class EnemySpawning : MonoBehaviour
{
    //obican spawn sa strana
    [SerializeField] private Transform playerTarget;
    [SerializeField] private EnemyMovement enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float minSpawnInterval = 2f;
    [SerializeField] private float maxSpawnInterval = 5f;

    private float spawnTimer;
    private bool canSpawn = false;
    private List<EnemyMovement> spawnedEnemies = new List<EnemyMovement>();

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
            EnemyMovement enemyMovement = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, transform);
            spawnedEnemies.Add(enemyMovement);
            spawnTimer = UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval);
        }
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