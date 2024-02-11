using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public CharacterController characterController;
    [HideInInspector] 
    public float currentSpeed;

    [SerializeField] private float walkAccelerationTime = 0.5f;
    [SerializeField] private float runAccelerationTime = 0.3f;
    [SerializeField] private float decelerationTime = 0.5f;
    [SerializeField] private float jumpStrength = 10f;
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 8f;
   
    private Vector3 currentMoveVelocity;
    private Vector3 currentForceVelocity;
    private Vector2 movementInput;
    private bool jumpPressed; 
    private float targetSpeed;
    private bool canPlayerMove = true;
    private CameraController cameraController;

    public HandItemSO currentHandItemSO;
    public GameObject currentItemGO;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        cameraController = GetComponentInChildren<CameraController>();
    }
    private void Start()
    {
        InputManager.Instance.OnMove += HandleMoveInput;
        InputManager.Instance.OnJump += HandleJumpInput;
        InputManager.Instance.StopPlayer += CanPlayerMove;

        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            inventory.OnHoldItem += SetCurrentHoldItem;
        }
    }
    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMove -= HandleMoveInput;
            InputManager.Instance.OnJump -= HandleJumpInput;
            InputManager.Instance.StopPlayer -= CanPlayerMove;

            Inventory inventory = FindObjectOfType<Inventory>();
            if (inventory != null)
            {
                inventory.OnHoldItem -= SetCurrentHoldItem;
            }
        }
    }

    private void Update()
    {
        if (canPlayerMove)
        {
            HandleMovement();
            HandleGravityAndJump();
        }
    }

    private void SetCurrentHoldItem(HandItemSO _item)
    {
        currentHandItemSO = _item;
        GetComponent<PlayerAnimatorController>().Equip();

        if (currentItemGO != null)
        {
            InputManager.Instance.OnItemUse -= currentItemGO.GetComponent<GrabbableItem>().Use;
            InputManager.Instance.OnSpecialUseItem -= currentItemGO.GetComponent<GrabbableItem>().SpecialUse;
        }
    }

    private void SpawnItem()
    {
        Destroy(currentItemGO);

        if (currentHandItemSO != null)
        {
            currentItemGO = Instantiate(currentHandItemSO.prefab, cameraController.GetHandTransform().position, Quaternion.identity);
            currentItemGO.transform.parent = cameraController.GetHandTransform();      
            currentItemGO.transform.localRotation = Quaternion.Euler(currentHandItemSO.startRotation);
            currentItemGO.GetComponent<Rigidbody>().isKinematic = true;

            InputManager.Instance.OnItemUse += currentItemGO.GetComponent<GrabbableItem>().Use;
            InputManager.Instance.OnSpecialUseItem += currentItemGO.GetComponent<GrabbableItem>().SpecialUse;
        }
    }

    private void CanPlayerMove(bool _canMove)
    {
        canPlayerMove = _canMove;
        if (!_canMove)
            currentSpeed = 0;
    }
    private void HandleMovement()
    {
        Vector3 moveVector = transform.TransformDirection(new Vector3(movementInput.x, 0, movementInput.y));
        bool isMoving = movementInput.sqrMagnitude > 0.01f;
        targetSpeed = isMoving ? (InputManager.Instance.IsRunning ? runSpeed : walkSpeed) : 0;

        if (isMoving)
        {
            float accelerationTime = InputManager.Instance.IsRunning ? runAccelerationTime : walkAccelerationTime;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * (targetSpeed / accelerationTime));
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * (walkSpeed / decelerationTime));
        }

        currentMoveVelocity = moveVector.normalized * currentSpeed;
        characterController.Move(currentMoveVelocity * Time.deltaTime);
    }

    private void HandleGravityAndJump()
    {
        if (characterController.isGrounded)
        {
            currentForceVelocity.y = -2f;

            if (jumpPressed)
            {
                currentForceVelocity.y = jumpStrength;
                jumpPressed = false;
            }
        }
        else
        {
            currentForceVelocity.y -= GameManager.instance.options.gravityStrength * Time.deltaTime;
        }

        characterController.Move(currentForceVelocity * Time.deltaTime);
    }

    private void HandleMoveInput(Vector2 _input)
    {
        movementInput = _input;
    }

    private void HandleJumpInput(bool _pressed)
    {
        jumpPressed = _pressed;
    }
}