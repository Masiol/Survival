using System;
using UnityEngine;

public class AnimalHealth : MonoBehaviour
{
    public float MaxHealth { get; set; }

    private float currentHealth;

    public event Action OnHealthChanged;
    public event Action OnHealthZero;

    public void InitializeHealth(float _maxHealth)
    {
        MaxHealth = _maxHealth;
        currentHealth = _maxHealth;
    }

    public void TakeDamage(float _damage)
    {
        currentHealth -= _damage;
        Debug.Log(_damage);
        currentHealth = Mathf.Clamp(currentHealth, 0f, MaxHealth);

        Debug.Log("Health: " + currentHealth);
        if (currentHealth > 0f)
            OnHealthChanged?.Invoke();

        if (currentHealth <= 0f)
        {
            OnHealthZero?.Invoke();
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}