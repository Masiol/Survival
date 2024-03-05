using UnityEngine;

public class PlayerStatusBar : MonoBehaviour
{
    [SerializeField] private StaminaBar staminaBar;
    [SerializeField] private HealthBar healthBar;

    private void Start()
    {
        SetStamina();
        SetHealth();
    }

    private void SetStamina()
    {
        StaminaManager.Instance.OnStaminaChange += UpdateStaminaBar;
        staminaBar.SetMaxStamina(StaminaManager.Instance.GetCurrentStamina());
    }

    private void SetHealth()
    {
        HealthManager.Instance.OnHealthChange += UpdateHealthBar;
        healthBar.SetMaxHealth(HealthManager.Instance.GetCurrentHealth());
    }

    private void UpdateStaminaBar(float currentStamina)
    {
        staminaBar.UpdateStaminaBar(currentStamina);
    }

    private void UpdateHealthBar(float currentHealth)
    {
        healthBar.UpdateHealthBar(currentHealth);
    }
}