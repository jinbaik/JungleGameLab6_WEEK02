using UnityEngine;

public class Glide : MonoBehaviour
{

    [SerializeField] private float baseSpeed = 30f;
    [SerializeField] private float maxThrustSpeed;
    [SerializeField] private float minThrustSpeed;
    [SerializeField] private float thrustFactor;
    [SerializeField] private float dragFactor;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float tiltStrength;
    [SerializeField] private float maxTiltAngle = 60;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float lowPercent = 0.1f;
    [SerializeField] private float highPercent = 1;
    [SerializeField] private float turnDragPenalty = 15f;

    private float currentThrustSpeed;
    private float currentTilt;

    private Transform cameraTransform;
    private CameraTracking cam;
    private Rigidbody rb;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform = Camera.main.transform.parent;
        rb = GetComponent<Rigidbody>();
        cam = cameraTransform.gameObject.GetComponent<CameraTracking>();
    }

    void Update()
    {
        ManageRotation();
    }

    void FixedUpdate()
    {
        GlidingMovement();
    }

    private void GlidingMovement()
    {
        float pitchInRads = transform.eulerAngles.x * Mathf.Deg2Rad;
        float mappedPitch = Mathf.Sin(pitchInRads) * thrustFactor;
        float offsetMappedPitch = Mathf.Cos(pitchInRads) * dragFactor;
        float pitchInDeg = transform.eulerAngles.x % 360;
        float accelerationPercent = pitchInDeg >= 300f ? lowPercent : highPercent;
        float tiltPercent = Mathf.Abs(currentTilt) / maxTiltAngle;
        Vector3 glidingForce = Vector3.forward * currentThrustSpeed;

        currentThrustSpeed += mappedPitch * accelerationPercent * Time.fixedDeltaTime;
        currentThrustSpeed -= tiltPercent * turnDragPenalty * Time.fixedDeltaTime;
        currentThrustSpeed = Mathf.Clamp(currentThrustSpeed, 0, maxThrustSpeed);

        if (rb.linearVelocity.magnitude >= minThrustSpeed)
        {
            rb.AddRelativeForce(glidingForce);
            rb.linearDamping = Mathf.Clamp(offsetMappedPitch, 0.2f, dragFactor);
        }
        else
        {
            currentThrustSpeed = 0;
        }


    }

    private void ManageRotation()
    {
        float mouseX = cam.lookInput.x;
        currentTilt += -mouseX * tiltStrength * Time.deltaTime;
        currentTilt = Mathf.Lerp(currentTilt, 0, tiltSpeed * Time.deltaTime);
        currentTilt = Mathf.Clamp(currentTilt, -maxTiltAngle, maxTiltAngle);

        Quaternion targetRotation = Quaternion.Euler(cameraTransform.eulerAngles.x, cameraTransform.eulerAngles.y, currentTilt);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
