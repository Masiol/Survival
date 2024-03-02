using System;
using UnityEngine;

public class StaminaManager : MonoBehaviour, IStaminaConsumer
{
    public static StaminaManager Instance { get; private set; }
    public event Action<float> OnStaminaChange;

    [SerializeField] private StaminaConfig config;
    private float currentStamina;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentStamina = config.maxStamina;
    }

    private void Update()
    {
        if (InputManager.Instance.IsRunning && InputManager.Instance.IsMoving)
        {
            HandleRun(true);
        }

        if (currentStamina < config.maxStamina)
        {
            RegenerateStamina(config.staminaRegenRate * Time.deltaTime);
        }
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public void ConsumeStamina(float amount)
    {
        currentStamina = Mathf.Max(currentStamina - amount, 0);
        OnStaminaChange?.Invoke(currentStamina);
    }

    private void RegenerateStamina(float amount)
    {
        currentStamina = Mathf.Min(currentStamina + amount, config.maxStamina);
        OnStaminaChange?.Invoke(currentStamina);
    }

    private void HandleRun(bool isRunning)
    {
        if (isRunning)
        {
            ConsumeStamina(config.runStaminaDrain * Time.deltaTime);
        }
    }
}
