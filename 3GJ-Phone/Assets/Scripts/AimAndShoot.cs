using UnityEngine;

public class AimAndShoot : MonoBehaviour
{
    [SerializeField] private float range = 10f;
    [SerializeField] private Transform weapon;
    [SerializeField] private Transform barrel;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootDelay = 0.3f;
    [SerializeField] private float bulletSpeed = 30f; 

    private GameObject enemy;
    private float nextShootTime;

    void Update()
    {
        FindEnemy();

        if (enemy != null)
        {
            RotateTowards();
            Shoot();
        }
    }

    void FindEnemy()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        enemy = null;
        float closestDistance = range;

        foreach (GameObject e in allEnemies)
        {
            float distance = Vector3.Distance(transform.position, e.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                enemy = e;
            }
        }
    }

    void RotateTowards()
    {
        Vector3 direction = (enemy.transform.position - weapon.position).normalized;
        weapon.right = direction;
    }

    void Shoot()
    {
        if (Time.time < nextShootTime) return;

        nextShootTime = Time.time + shootDelay;

        GameObject newBullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation);

       
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = barrel.right * bulletSpeed;
        }

        Destroy(newBullet, 3f);  
    }
}