using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public CharacterController characterController;

    [SerializeField] private float walkAccelerationTime = 0.5f;
    [SerializeField] private float runAccelerationTime = 0.3f;
    [SerializeField] private float decelerationTime = 0.5f;
    [SerializeField] private float jumpStrength = 10f;
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 8f;
    public float currentSpeed;

    private Vector3 currentMoveVelocity;
    private Vector3 currentForceVelocity;
    private Vector2 movementInput;
    private bool jumpPressed; 
    private float targetSpeed;
    private bool canPlayerMove = true;

    private CameraController cameraController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraController = GetComponentInChildren<CameraController>();

        InputManager.Instance.OnMove += HandleMoveInput;
        InputManager.Instance.OnJump += HandleJumpInput;
        InputManager.Instance.StopPlayer += CanPlayerMove;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMove -= HandleMoveInput;
            InputManager.Instance.OnJump -= HandleJumpInput;
            InputManager.Instance.StopPlayer -= CanPlayerMove;
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
    public void DropObject(Item _item)
    {
        if (_item == null || _item.itemQuantity <= 0 || _item.prefab == null)
        {
            Debug.Log("_item == null || _item.itemQuantity <= 0 || _item.prefab == null");
            return;
        }

        GameObject dropObject = Instantiate(_item.prefab, cameraController.GetDropPoint().position, cameraController.GetDropPoint().rotation);
        Debug.Log("should drop");
        if(dropObject.GetComponent<Rigidbody>())
            return;
        else
            dropObject.AddComponent<Rigidbody>();
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