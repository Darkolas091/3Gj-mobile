using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject hitVFX; // Optional hit effect

    private void OnTriggerEnter(Collider other)
    {
        // Check if hit an enemy
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            
            // Spawn hit VFX if assigned
            if (hitVFX != null)
            {
                Instantiate(hitVFX, transform.position, Quaternion.identity);
            }
            
            // Play hit sound
            SoundEffectManager.PlaySoundEffect("Hit");
            
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if hit an enemy
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            
            // Spawn hit VFX if assigned
            if (hitVFX != null)
            {
                Instantiate(hitVFX, transform.position, Quaternion.identity);
            }
            
            // Play hit sound
            SoundEffectManager.PlaySoundEffect("Hit");
            
            Destroy(gameObject);
        }
    }
}

