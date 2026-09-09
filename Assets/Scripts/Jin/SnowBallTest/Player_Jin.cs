using UnityEngine;
using UnityEngine.InputSystem;

public enum ScaleEnum { low, middle, high };

public class Player_Jin : MonoBehaviour
{
    [SerializeField] float forwardSpeed = 1000f;
    [SerializeField] float turnSpeed = 90f;
    [SerializeField] float jumpForce = 50f;
    [SerializeField] private Transform visualSphere;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Rigidbody rb;
    private bool isFlying;
    public CameraPrototype cam;
    public ScaleEnum size = ScaleEnum.middle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        size = ScaleEnum.middle;
        // rb.linearDamping = 0.5f;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

    }

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
        cam.CameraRotation(lookInput);
    }
    void OnAttack(InputValue value)
    {
        DecreaseScale();
    }
    void OnAbility(InputValue value)
    {

        IncreaseScale();
    }
    void OnJump(InputValue value)
    {
        Jump();
    }

    private void Jump()
    {
        if (isFlying) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        // rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    private void IncreaseScale()
    {
        if (size == ScaleEnum.high) return;
        size = (ScaleEnum)((int)size + 1);
        transform.localScale = transform.localScale * 4;
    }
    private void DecreaseScale()
    {
        if (size == ScaleEnum.low) return;
        size = (ScaleEnum)((int)size - 1);
        transform.localScale = transform.localScale / 4;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentVelocity = rb.linearVelocity;
        float currentSpeed = currentVelocity.magnitude;

        if (currentSpeed < 0.1f && moveInput.magnitude > 0.1f)
        {
            currentVelocity = transform.forward * forwardSpeed;
            currentSpeed = forwardSpeed;
        }

        if (Mathf.Abs(moveInput.x) > 0.01f)
        {
            float rotationAngle = moveInput.x * turnSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0, rotationAngle, 0);
            currentVelocity = turnRotation * currentVelocity;
        }

        if (moveInput.y > 0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, forwardSpeed, Time.fixedDeltaTime);
        }

        rb.linearVelocity = currentVelocity.normalized * currentSpeed;

        RotateVisualSphere(rb.linearVelocity);



        // Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        // rb.AddForce(moveDir * forwardSpeed * Time.fixedDeltaTime);

        cam.FollowTargetTransform(rb.linearVelocity);

    }

    void RotateVisualSphere(Vector3 velocity)
    {
        if (velocity.magnitude > 0.1f)
        {
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, velocity.normalized);
            float radius = transform.localScale.x * 0.5f;
            float rotaionAmount = (velocity.magnitude * Time.fixedDeltaTime / radius) * Mathf.Rad2Deg;
            visualSphere.Rotate(rotationAxis, rotaionAmount, Space.World);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            isFlying = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            isFlying = true;
        }
    }
}
