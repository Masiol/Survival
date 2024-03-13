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

    private void UpdateStaminaBar(float _currentStamina)
    {
        staminaBar.UpdateStaminaBar(_currentStamina);
    }
   

    private void UpdateHealthBar(float _currentHealth)
    {
        healthBar.UpdateHealthBar(_currentHealth);
    }
}