using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxhealth = 10;
    [SerializeField] private EnemyHealthBar healthBar; // Reference to the health bar
    
    private int currentHealth;
    private bool isDead = false;
    public bool IsDead => isDead;

    private void Start()
    {
        currentHealth = maxhealth;
        
        // Initialize health bar if assigned
        if (healthBar != null)
        {
            healthBar.Initialize(maxhealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        
        // Update health bar
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        
        // Play death sound
        SoundEffectManager.PlaySoundEffect("EnemyDeath");
        
        // TODO: Spawn death VFX, drop currency/items, etc.
        
        // Destroy the enemy
        Destroy(gameObject);
    }
}