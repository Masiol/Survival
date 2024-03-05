using UnityEngine;
using System;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }
    public event Action<float> OnHealthChange;

    [SerializeField] private HealthConfig config;

    private IHealth playerHealth;
    //private DamageController damageController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerHealth = new Health(config.maxHealth);
        playerHealth.OnHealthChange += (currentHealth) => OnHealthChange?.Invoke(currentHealth);
       // damageController = new DamageController(playerHealth);
    } 
    public void ReceiveDamage(int damage)
    {
      //  damageController.DealDamage(damage);
    }

    public void ChangeHealth(float amount)
    {
        playerHealth.ChangeHealth(amount);
    }
   
    public float GetCurrentHealth()
    {
        return playerHealth.GetCurrentHealth();
    }

    public void RegenerateHealth(float amount)
    {
        ChangeHealth(amount);
    }
}
