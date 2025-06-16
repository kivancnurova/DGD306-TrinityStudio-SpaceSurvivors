using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    private bool isDead = false;

    public UnityEvent<int, int> onHealthChanged;
    public UnityEvent onDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (EndGameController.Instance != null)
        {
            EndGameController.Instance.OnPlayerKilled(gameObject.tag);
        }
        onDeath?.Invoke();
        Destroy(gameObject);
    }
}