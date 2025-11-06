using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHearts = 3;
    [SerializeField] private int currentHearts;
    [SerializeField] private Image[] heartIcons;

    private void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartUI();
    }

    public void TakeDamage(int damage)
    {
        currentHearts -= damage;
        if (currentHearts < 0) currentHearts = 0;
        
        SoundEffectManager.PlaySoundEffect("PlayerHit");
        
        UpdateHeartUI();
        
        if (currentHearts == 0)
        {
            Die();
        }
    }
    
    public void TakeOneDamage()
    {
        TakeDamage(1);
    }

    private void Die()
    {
        Debug.Log("Player Dead");
        
        SoundEffectManager.PlaySoundEffect("PlayerDeath");
        
         GameOverManager.Instance?.TriggerGameOver(GameOverManager.LoseReason.PlayerDied);

    }

    private void UpdateHeartUI()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].enabled = i < currentHearts;
        }
    }

    public void Heal(int amount)
    {
        currentHearts += amount;
        if (currentHearts > maxHearts) currentHearts = maxHearts;
        UpdateHeartUI();
    }
}