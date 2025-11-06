using UnityEngine;
using UnityEngine.SceneManagement;

public class TombstoneHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
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
        SoundEffectManager.PlaySoundEffect("TombstoneHit");
        
        Debug.Log($"Tombstone health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Play destruction sound
        SoundEffectManager.PlaySoundEffect("TombstoneDestroyed");
        
        // Spawn destruction VFX if assigned
        if (destroyedVFX != null)
        {
            Instantiate(destroyedVFX, transform.position, Quaternion.identity);
        }
        
        // Trigger game over
        GameOverManager.Instance?.TriggerGameOver(GameOverManager.LoseReason.TombstoneDestroyed);
        
        Destroy(gameObject);
    }
}


