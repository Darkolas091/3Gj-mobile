using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private int damage = 1;

    private Transform target;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        FindTarget();
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                movementSpeed * Time.deltaTime);
        }

        // Always look at camera
        if (mainCamera != null)
        {
            transform.LookAt(mainCamera.transform);
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Ghost OnTriggerEnter with: {other.gameObject.name}");
        HandleContact(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Ghost OnCollisionEnter with: {collision.gameObject.name}");
        HandleContact(collision.gameObject);
    }

    private void HandleContact(GameObject contactObject)
    {
        var playerTarget = contactObject.GetComponent<PlayerTarget>();
        var towerTarget = contactObject.GetComponent<TowerTarget>();
        var tombstoneTarget = contactObject.GetComponent<TombstoneTarget>();
        
        if (playerTarget != null)
        {
            Debug.Log("Ghost hit player!");
            var playerHealth = contactObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Dealt {damage} damage to player");
            }
            Die();
        }
        else if (towerTarget != null)
        {
            Debug.Log("Ghost hit tower!");
            var towerHealth = contactObject.GetComponent<TowerHealth>();
            if (towerHealth != null)
            {
                towerHealth.TakeDamage(damage);
                Debug.Log($"Dealt {damage} damage to tower");
            }
            Die();
        }
        else if (tombstoneTarget != null)
        {
            Debug.Log("Ghost hit tombstone!");
            var tombstoneHealth = contactObject.GetComponent<TombstoneHealth>();
            if (tombstoneHealth != null)
            {
                tombstoneHealth.TakeDamage(damage);
                Debug.Log($"Dealt {damage} damage to tombstone");
            }
            Die();
        }
        else
        {
            Debug.Log($"Ghost contacted {contactObject.name} but found no valid target component");
        }
    }

    private void Die()
    {
        Debug.Log("Ghost dying");
        Destroy(gameObject);
    }
}
