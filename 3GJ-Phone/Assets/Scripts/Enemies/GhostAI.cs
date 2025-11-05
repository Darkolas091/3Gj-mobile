using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private int damage = 1;

    private Transform target;

    private void Update()
    {
        FindTarget();
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                movementSpeed * Time.deltaTime);
            transform.LookAt(target.position);
        }
    }

    private void FindTarget()
    {
        // Check for player in detectionRadius
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        Transform player = null;
        float playerDist = float.MaxValue;
        foreach (var hit in hits)
        {
            var playerTarget = hit.GetComponent<PlayerTarget>();
            if (playerTarget != null)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < playerDist)
                {
                    player = hit.transform;
                    playerDist = dist;
                }
            }
        }
        // Find nearest tower (infinite range)
        Transform nearestTower = null;
        float towerDist = float.MaxValue;
        var towers = Object.FindObjectsByType<TowerTarget>(FindObjectsSortMode.None);
        foreach (var tower in towers)
        {
            float dist = Vector3.Distance(transform.position, tower.transform.position);
            if (dist < towerDist)
            {
                towerDist = dist;
                nearestTower = tower.transform;
            }
        }
        // Find nearest tombstone (infinite range)
        Transform nearestTombstone = null;
        float tombstoneDist = float.MaxValue;
        var tombstones = Object.FindObjectsByType<TombstoneTarget>(FindObjectsSortMode.None);
        foreach (var tombstone in tombstones)
        {
            float dist = Vector3.Distance(transform.position, tombstone.transform.position);
            if (dist < tombstoneDist)
            {
                tombstoneDist = dist;
                nearestTombstone = tombstone.transform;
            }
        }
        // Prioritize: Player (if in range) > Tower > Tombstone
        if (player != null)
            target = player;
        else if (nearestTower != null)
            target = nearestTower;
        else if (nearestTombstone != null)
            target = nearestTombstone;
    }

    private void OnCollisionEnter(Collision other)
    {
        var playerTarget = other.gameObject.GetComponent<PlayerTarget>();
        var towerTarget = other.gameObject.GetComponent<TowerTarget>();
        var tombstoneTarget = other.gameObject.GetComponent<TombstoneTarget>();
        if (playerTarget != null)
        {
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);
            Die();
        }
        else if (towerTarget != null || tombstoneTarget != null)
        {
            // TODO: Implement tower/tombstone health
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
