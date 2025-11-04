using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    //skripta za spawnanje enemija s neba(in case is needed lol)
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private float secondsBetweenAsteroids = 1.5f;
    [SerializeField] private Vector2 forceRange;

    private Camera mainCamera;

    private float timer;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnEnemi();

            timer += secondsBetweenAsteroids;
        }
    }

    private void SpawnEnemi()
    {
        int side = Random.Range(0, 4);

        Vector3 spawnPoint = Vector2.zero;
        Vector2 direction = Vector2.zero;
        switch (side)
        {
            case 0:
                spawnPoint.x = 0;
                spawnPoint.y = Random.value;
                direction = new Vector2(1f, Random.Range(-1f, 1f));
                break;
            case 1:
                spawnPoint.x = 1;
                spawnPoint.y = Random.value;
                direction = new Vector2(-1f, Random.Range(-1f, 1f));
                break;
            case 2:
                spawnPoint.x = Random.value;
                spawnPoint.y = 0;
                direction = new Vector2(Random.Range(-1f, 1f), 1f);
                break;
            case 3:
                spawnPoint.x = Random.value;
                spawnPoint.y = 1;
                direction = new Vector2(Random.Range(-1f, 1f), -1f);
                break;
        }

        float zDistance = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        spawnPoint.z = zDistance;
        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = 0;

        GameObject selectedAsteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
        GameObject tempAsteroid =
            Instantiate(
                selectedAsteroid,
                worldSpawnPoint,
                Quaternion.identity
                );
        Rigidbody asteroidBody = tempAsteroid.GetComponent<Rigidbody>();
        asteroidBody.linearVelocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);

    }
}
