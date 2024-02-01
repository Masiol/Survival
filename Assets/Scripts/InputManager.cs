using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    public event System.Action<Vector2> OnMove;
    public event System.Action<Vector2> OnLook;
    public event System.Action<bool> OnJump;
    public event System.Action<bool> OnInventory;
    public event System.Action<bool> StopPlayer;

    public event System.Action OnInteract;
    public event System.Action OnPickup;
    public event System.Action OnHold;
    public bool IsRunning { get; private set; }
    public bool IsMoving { get; private set; }
    public float StrafeDirection { get; private set; }


    private bool inventoryToggle = false;

    private PlayerInputSystem inputSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupInputSystem();
    }

    private void SetupInputSystem()
    {
        inputSystem = new PlayerInputSystem();

        inputSystem.Player.Move.performed += ctx =>
        {
            Vector2 moveInput = ctx.ReadValue<Vector2>();
            IsMoving = moveInput.magnitude > 0;
            OnMove?.Invoke(moveInput);
            StrafeDirection = moveInput.x;
        };
        inputSystem.Player.Move.canceled += ctx =>
        {
            IsMoving = false;
            OnMove?.Invoke(Vector2.zero);
            StrafeDirection = 0;
        };

        inputSystem.Player.Look.performed += ctx => OnLook?.Invoke(ctx.ReadValue<Vector2>());
        inputSystem.Player.Look.canceled += ctx => OnLook?.Invoke(Vector2.zero);

        inputSystem.Player.Jump.performed += ctx => OnJump?.Invoke(true);
        inputSystem.Player.Jump.canceled += ctx => OnJump?.Invoke(false);

        inputSystem.Player.Run.performed += ctx => IsRunning = true;
        inputSystem.Player.Run.canceled += ctx => IsRunning = false;

        inputSystem.Player.Interact.performed += ctx => OnInteract?.Invoke();
        inputSystem.Player.Pickup.performed += ctx => OnPickup?.Invoke();
        inputSystem.Player.Hold.performed += ctx => OnHold?.Invoke();

        inputSystem.Player.Inventory.performed += ctx => ToggleInventory();
    } 
    private void OnEnable()
    {
        inputSystem.Player.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Player.Disable();
    }

    private void ToggleInventory()
    {
        inventoryToggle = !inventoryToggle;
        OnInventory?.Invoke(inventoryToggle);

        StopPlayer?.Invoke(!inventoryToggle);
    }
}
