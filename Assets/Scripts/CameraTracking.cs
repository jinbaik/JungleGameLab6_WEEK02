using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTracking : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float smoothTime;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float gamepadSensitivity = 100f;
    [SerializeField] private float maxViewRange;
    private float mouseX, mouseY;
    [SerializeField] private bool isGamePad;

    [SerializeField] private Vector3 offset;
    private Vector3 currentVelocity;
    public Vector2 lookInput;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    void OnEnable()
    {
        inputActions.Enable();
    }
    void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();

        isGamePad = ctx.control.device is Gamepad;
    }

    private void FixedUpdate()
    {
        FollowTargetTransform();
    }
    private void Update()
    {

        CameraRotation();
    }

    private void FollowTargetTransform()
    {
        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 positionInterpolation = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);

        transform.position = positionInterpolation;
    }
    private void CameraRotation()
    {
        lookInput = inputActions.Player.Look.ReadValue<Vector2>();

        if (isGamePad)
        {
            mouseX -= lookInput.y * gamepadSensitivity * Time.deltaTime;
            mouseY += lookInput.x * gamepadSensitivity * Time.deltaTime;
        }
        else
        {
            mouseX -= lookInput.y * mouseSensitivity;
            mouseY += lookInput.x * mouseSensitivity;
        }




        float clampedX = Mathf.Clamp(mouseX, -maxViewRange, maxViewRange);

        Quaternion targetRotation = Quaternion.Euler(clampedX, mouseY, transform.eulerAngles.z);
        transform.rotation = targetRotation;
    }
}