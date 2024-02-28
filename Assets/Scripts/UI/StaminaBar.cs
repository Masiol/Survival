using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Slider staminaSlider;

    private void Start()
    {
        staminaSlider = GetComponent<Slider>();
    }

    public void UpdateStaminaBar(float currentStamina)
    {
        staminaSlider.value = currentStamina;
    }

    public void SetMaxStamina(float maxStamina)
    {
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
    }
}