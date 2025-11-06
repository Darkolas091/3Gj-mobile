using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxhealth = 10;
    private int currentHealth;
    private bool isDead = false;
    public bool IsDead => isDead;

    private void Start()
    {
        currentHealth = maxhealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

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