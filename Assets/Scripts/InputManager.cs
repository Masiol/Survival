using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    public event System.Action<Vector2> OnMove;
    public event System.Action<Vector2> OnLook;
    public event System.Action<bool> OnJump;
    public event System.Action OnInventory;
    public event System.Action<bool> StopPlayer;

    public event System.Action OnInteract;
    public event System.Action OnPickup;
    public event System.Action OnGrab;
    public event System.Action Cancel;
    public event System.Action OnInventoryDropItem;
    public event System.Action<int> OnInteractHotBar;

    public event System.Action OnItemUse;
    public bool IsRunning { get; private set; }
    public bool IsMoving { get; private set; }
    public float StrafeDirection { get; private set; }

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
        inputSystem.Player.Hold.performed += ctx => OnGrab?.Invoke();

        inputSystem.Player.Escape.performed += ctx => Cancel?.Invoke();

        inputSystem.Player.UseItem.performed += ctx => OnItemUse?.Invoke();

        //zmienic na UI
        inputSystem.Player.InventoryDropItem.performed += ctx => OnInventoryDropItem?.Invoke();
        inputSystem.Player.Inventory.performed += ctx => OnInventory?.Invoke();
        //zmienic na UI
        inputSystem.UI.Key1.performed += ctx => OnInteractHotBar.Invoke(1);
        inputSystem.UI.Key2.performed += ctx => OnInteractHotBar.Invoke(2);
        inputSystem.UI.Key3.performed += ctx => OnInteractHotBar.Invoke(3);
        inputSystem.UI.Key4.performed += ctx => OnInteractHotBar.Invoke(4);
        inputSystem.UI.Key5.performed += ctx => OnInteractHotBar.Invoke(5);
        inputSystem.UI.Key6.performed += ctx => OnInteractHotBar.Invoke(6);
        inputSystem.UI.Key7.performed += ctx => OnInteractHotBar.Invoke(7);

    }
    private void OnEnable()
    {
        inputSystem.Player.Enable();
        inputSystem.UI.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Player.Disable();
        inputSystem.UI.Disable();
    }
}
