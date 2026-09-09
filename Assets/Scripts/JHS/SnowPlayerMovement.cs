using UnityEngine;
using UnityEngine.InputSystem;

public class SnowPlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    public float speed = 5.0f;
    public GameObject snowBall;
    private bool isGrounded;
    private Vector3 velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnAttack(InputValue value)
    {
        // Instantiate(snowBall);
        snowBall.SetActive(true);
    }

    void OnJump(InputValue value)
    {
        snowBall.GetComponent<PlayerSnowBall>().SetConnectBool(false);
        Destroy(snowBall, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(moveDir * speed * Time.deltaTime);

        velocity.y += -9.81f * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
