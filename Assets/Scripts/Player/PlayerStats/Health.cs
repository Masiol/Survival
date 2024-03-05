using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : IHealth
{
    private float maxHealth;
    private float currentHealth;

    public event Action<float> OnHealthChange;

    public Health(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void ChangeHealth(float amount)
    {
        float newValue = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        currentHealth = newValue;
        OnHealthChange?.Invoke(currentHealth);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}