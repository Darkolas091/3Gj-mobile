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

    private void Start()
    {
        spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void Update()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }

        if (spawnTimer <= 0)
        {
            EnemyMovement enemyMovement = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, transform);

            enemyMovement.playerTarget = playerTarget;

            spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }
}