using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private GameObject destroyedVFX;
    
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // Play damage sound
        SoundEffectManager.PlaySoundEffect("TowerHit");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Play destruction sound
        SoundEffectManager.PlaySoundEffect("TowerDestroyed");
        
        // Spawn destruction VFX if assigned
        if (destroyedVFX != null)
        {
            Instantiate(destroyedVFX, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}