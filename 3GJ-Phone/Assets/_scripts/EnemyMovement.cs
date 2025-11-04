using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform playerTarget;
    [SerializeField] public float movementSpeed = 10f;
    [SerializeField] private float damage = 20f;
    
    private void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            playerTarget.position,
            movementSpeed * Time.deltaTime);
        transform.LookAt(playerTarget.position);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
