using UnityEngine;

public class PlayerStatusBar : MonoBehaviour
{
    [SerializeField] private StaminaBar staminaBar; // UI component for stamina

    private void Start()
    {
        StaminaManager.Instance.OnStaminaChange += UpdateStaminaBar;
        staminaBar.SetMaxStamina(StaminaManager.Instance.GetCurrentStamina());
    }
    private void UpdateStaminaBar(float currentStamina)
    {
        staminaBar.UpdateStaminaBar(currentStamina);
    }
}