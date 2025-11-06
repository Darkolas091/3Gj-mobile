using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private WeaponType weaponType = WeaponType.Slingshot;
    
    [Header("Ranged Weapon (Slingshot)")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootRange = 10f;
    [SerializeField] private float shootCooldown = 0.5f;
    [SerializeField] private float bulletSpeed = 20f;
    
    [Header("Melee Weapon (Shovel)")]
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private float meleeCooldown = 0.8f;
    [SerializeField] private int meleeDamage = 2;
    [SerializeField] private LayerMask enemyLayer;
    
    [Header("VFX")]
    [SerializeField] private GameObject muzzleFlashVFX;
    [SerializeField] private GameObject meleeSwingVFX;
    
    private float nextAttackTime;
    private GameObject currentTarget;

    public enum WeaponType
    {
        Slingshot,
        Shovel
    }

    private void Update()
    {
        FindClosestEnemy();
        
        if (currentTarget != null && Time.time >= nextAttackTime)
        {
            Attack();
        }
    }

    private void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        currentTarget = null;
        float closestDistance = weaponType == WeaponType.Slingshot ? shootRange : meleeRange;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = enemy;
            }
        }
    }

    private void Attack()
    {
        if (weaponType == WeaponType.Slingshot)
        {
            ShootProjectile();
            nextAttackTime = Time.time + shootCooldown;
        }
        else if (weaponType == WeaponType.Shovel)
        {
            MeleeAttack();
            nextAttackTime = Time.time + meleeCooldown;
        }
    }

    private void ShootProjectile()
    {
        if (bulletPrefab == null || firePoint == null || currentTarget == null) return;

        // Calculate direction to target at the moment of firing
        Vector3 direction = (currentTarget.transform.position - firePoint.position).normalized;
        
        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        
        // Set bullet velocity
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
        
        // Spawn muzzle flash VFX
        if (muzzleFlashVFX != null)
        {
            Instantiate(muzzleFlashVFX, firePoint.position, firePoint.rotation);
        }
        
        // Play shoot sound
        SoundEffectManager.PlaySoundEffect("Shoot");
        
        // Auto-destroy bullet after 3 seconds
        Destroy(bullet, 3f);
    }

    private void MeleeAttack()
    {
        if (currentTarget == null) return;

        // Check if target is still in range
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distance > meleeRange) return;

        // Deal damage to enemy
        EnemyHealth enemyHealth = currentTarget.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(meleeDamage);
        }
        
        // Spawn melee swing VFX
        if (meleeSwingVFX != null)
        {
            Vector3 midPoint = (transform.position + currentTarget.transform.position) / 2f;
            Instantiate(meleeSwingVFX, midPoint, Quaternion.identity);
        }
        
        // Play melee sound
        SoundEffectManager.PlaySoundEffect("MeleeSwing");
    }

    // Method to switch weapons (can be called from UI or item selection)
    public void SwitchWeapon(WeaponType newWeaponType)
    {
        weaponType = newWeaponType;
        Debug.Log($"Switched to {weaponType}");
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize weapon range in editor
        Gizmos.color = weaponType == WeaponType.Slingshot ? Color.yellow : Color.red;
        float range = weaponType == WeaponType.Slingshot ? shootRange : meleeRange;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

