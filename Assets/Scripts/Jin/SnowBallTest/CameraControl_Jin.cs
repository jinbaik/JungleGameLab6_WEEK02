using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl_Jin : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float smoothTime = 0;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float gamepadSensitivity = 100f;
    [SerializeField] private float maxViewRange = 45;
    private float mouseX, mouseY;
    [SerializeField] private bool isGamePad;

    [SerializeField] private Vector3 offset;
    private Vector3 currentVelocity;

    void Awake()
    {

    }

    private void CheckDeviceType(InputAction.CallbackContext ctx)
    {
        isGamePad = ctx.control.device is Gamepad;
    }


    private void FixedUpdate()
    {
        // FollowTargetTransform();
    }
    private void Update()
    {
        // CameraRotation();
    }

    public void FollowTargetTransform(Vector3 moveDir)
    {
        if (moveDir.sqrMagnitude < 0.001f)
        {
            moveDir = playerTransform.forward;
        }

        Quaternion lookRotation = Quaternion.LookRotation(moveDir.normalized);
        Vector3 rotatedOffset = lookRotation * offset;

        Vector3 desiredPosition = playerTransform.position + rotatedOffset;
        Vector3 positionInterpolation = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
        transform.rotation = lookRotation;
        transform.position = positionInterpolation;
    }
    public void CameraRotation(Vector2 lookInput)
    {
        if (isGamePad)
        {
            mouseY += lookInput.x * gamepadSensitivity * Time.deltaTime;
        }
        else
        {
            mouseY += lookInput.x * mouseSensitivity;
        }


        // Quaternion quaternion = new Quaternion();
        // quaternion.x = 0;
        // quaternion.y = playerTransform.rotation.y;
        // quaternion.z = 0;
        // transform.rotation = quaternion;

        float clampedY = Mathf.Clamp(mouseY, -maxViewRange, maxViewRange);

        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, clampedY, transform.eulerAngles.z);
        transform.rotation = targetRotation;
    }
}
