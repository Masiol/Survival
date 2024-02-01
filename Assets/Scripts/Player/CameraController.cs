using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform cameraHolder = null;
    [SerializeField] private Transform dropPoint = null;
    [SerializeField] private Transform camera = null;
    [Space]
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float minVerticalAngle = -60f;
    [Space]

    [SerializeField] private bool enableBobbing = true;
    [Range(0.001f, 0.01f)]
    [SerializeField] private float amount = 0.002f;
    [Range(1f, 30f)]
    [SerializeField] private float frequency = 10.0f;
    [Range(10f, 100f)]
    [SerializeField] private float smooth = 10.0f;

    [Space]
    [SerializeField] private float tiltAngle = 10f;
    [SerializeField] private float tiltSmoothTime = 0.1f;

    private float xRotation = 0f;
    private Vector2 lookInput;
    private Vector2 currentLookDelta;
    private Vector2 lookDeltaVelocity;
    private float tiltVelocity;
    private Vector3 cameraPos;

    private bool canPlayerRotateCamera = true;


    private void Start()
    {
        InputManager.Instance.OnLook += HandleLookInput;
        InputManager.Instance.StopPlayer += CanRotateCamera;
        cameraPos = transform.localPosition;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnLook -= HandleLookInput;
            InputManager.Instance.StopPlayer -= CanRotateCamera;
        }
    }

    private void Update()
    {
        if (!canPlayerRotateCamera)
            return;
        else
        { 
            HandleLook();
            if (enableBobbing)
            {
                HandleHeadBobbing();
            }
            HandleCameraTilt();
        }
    }

    public Transform GetDropPoint()
    {
        return dropPoint;
    }

    private void CanRotateCamera(bool _canRotate)
    {
        canPlayerRotateCamera = _canRotate;
    }

    private void HandleLook()
    {
        currentLookDelta = Vector2.SmoothDamp(currentLookDelta, lookInput, ref lookDeltaVelocity, smoothTime);

        float mouseX = currentLookDelta.x * GameManager.instance.options.MouseSensitivity * Time.deltaTime;
        float mouseY = currentLookDelta.y * GameManager.instance.options.MouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void HandleCameraTilt()
    {
        float tiltDirection = InputManager.Instance.StrafeDirection;
        float targetTilt = -tiltDirection * tiltAngle;
        float currentTilt = Mathf.SmoothDampAngle(cameraHolder.localEulerAngles.z, targetTilt, ref tiltVelocity, tiltSmoothTime);

        cameraHolder.localRotation = Quaternion.Euler(cameraHolder.localEulerAngles.x, cameraHolder.localEulerAngles.y, currentTilt);
    }

    private void HandleHeadBobbing()
    {
        if (enableBobbing)
        {
            float speed = GetComponentInParent<PlayerController>().currentSpeed;

            if (speed > 0)
            {
                StartHeadBob();
            }
            else
                StopHeadbob();
        }
    }
    private Vector3 StartHeadBob()
    {

        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * amount * 1.4f, smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * frequency / 2f) * amount * 1.6f, smooth * Time.deltaTime);
        transform.localPosition += pos;
        return pos;
    }
    private void StopHeadbob()
    {
        if (transform.localPosition == cameraPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, cameraPos, 1 * Time.deltaTime);
    }

    private void HandleLookInput(Vector2 _input)
    {
        lookInput = _input;
    }
}
