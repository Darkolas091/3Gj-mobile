using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 10f;
    [SerializeField] private float detectionRadius = 15f;
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
        // Prioritize: Player > Tower > Tombstone
        PlayerTarget[] players = Object.FindObjectsByType<PlayerTarget>(FindObjectsSortMode.None);
        TowerTarget[] towers = Object.FindObjectsByType<TowerTarget>(FindObjectsSortMode.None);
        TombstoneTarget[] tombstones = Object.FindObjectsByType<TombstoneTarget>(FindObjectsSortMode.None);
        Transform nearestPlayer = GetNearest(players);
        Transform nearestTower = GetNearest(towers);
        Transform nearestTombstone = GetNearest(tombstones);
        if (nearestPlayer != null && Vector3.Distance(transform.position, nearestPlayer.position) <= detectionRadius)
            target = nearestPlayer;
        else if (nearestTower != null)
            target = nearestTower;
        else if (nearestTombstone != null)
            target = nearestTombstone;
    }

    private Transform GetNearest(Component[] targets)
    {
        Transform nearest = null;
        float minDist = float.MaxValue;
        foreach (var t in targets)
        {
            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = t.transform;
            }
        }
        return nearest;
    }

    private void OnCollisionEnter(Collision other)
    {
        var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.GetComponent<TowerTarget>() || other.gameObject.GetComponent<TombstoneTarget>())
        {
            // TODO: Implement tower/tombstone health if needed
            Destroy(gameObject);
        }
    }
}
