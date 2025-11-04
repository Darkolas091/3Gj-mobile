using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
   [SerializeField] private Image healthBar;
   [SerializeField] private float maxHealth;
   
   [SerializeField] private float currentHealth;
   private bool isInFire;
   private bool hasHealStarted;
   private float damageTimer;
  

   private void Start()
   {
      currentHealth = maxHealth;
      healthBar.fillAmount = currentHealth / maxHealth;
   }

   private void Update()
   {
      DamagePerSecond();
      HealPerSecond();
   }

 /*  private void HealPerSecond()
   {
      damageTimer += Time.deltaTime;

      if (hasHealStarted && damageTimer >= 5f)
      {
         StartHealing(3);
         hasHealStarted = false;
         
      }
   }
*/
   IEnumerator HealPerSecond()
   {
      while (damageTimer > 3)
      {
         damageTimer += Time.deltaTime;
         yield return new WaitForEndOfFrame();
         StartHealing(3);
         
      }
     // yield return new WaitForSeconds(1f);
      
   }

   private void DamagePerSecond()
   {
      damageTimer += Time.deltaTime;

      if (isInFire && damageTimer >= 1f)
      {
         TakeDamage(1);
         damageTimer = 0;
      }
   }

   public void TakeDamage(float damage)
   {
      currentHealth -= damage;
      healthBar.fillAmount = currentHealth / maxHealth;
      if (currentHealth <= 0)
      {
         Debug.Log("Player Dead");
      }
   }

   public void StartHealing(float healAmount)
   {
      if (currentHealth > maxHealth)
      {
         currentHealth = maxHealth;
         return;
      }
     // currentHealth += healAmount;
    currentHealth =  Mathf.Lerp(currentHealth, currentHealth + healAmount, 1);
      healthBar.fillAmount = currentHealth / maxHealth;
   }

   public void FireDamage(bool inFire)
   {
      isInFire = inFire;
      damageTimer = 0;
   } 
   
   public void Healing(bool isHealing)
   {
      hasHealStarted = isHealing;
      damageTimer = 0;
      StartCoroutine(HealPerSecond());
   }
}
