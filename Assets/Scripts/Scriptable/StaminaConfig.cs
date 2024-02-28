using UnityEngine;

[CreateAssetMenu(fileName = "StaminaConfig", menuName = "Config/StaminaConfig")]
public class StaminaConfig : ScriptableObject
{
    public float maxStamina = 100f;
    public float staminaRegenRate = 5f;
    public float runStaminaDrain = 10f;
    public float jumpStaminaCost = 20f;
    public float useItemStaminaCost = 5f;
    public float specialUseItemStaminaCost = 10f;
}