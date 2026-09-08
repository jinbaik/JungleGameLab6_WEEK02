using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Flight Settings")]
    public float forwardThrust = 5f;      // 초기/지속 전방 추진력 (필요시)
    public float liftFactor = 2.5f;       // 양력 계수 (높을수록 잘 떠오름)
    public float dragFactor = 0.5f;       // 항력 계수 (높을수록 공기 저항 강함)

    [Header("Control Responsiveness")]
    public float pitchSpeed = 40f;        // 마우스/키보드 상하 회전 속도
    public float rollSpeed = 60f;         // 좌우 회전 속도

    private Rigidbody rb;
    private float pitchInput;
    private float rollInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 현실적인 중력 가속도 및 물리 설정 적용
        rb.useGravity = true;
        rb.linearDamping = 0.05f;  // 유니티 기본 드래그는 낮추고 코드로 제어
        rb.angularDamping = 1f;
    }


    private void OnMove(InputValue value)
    {
        Vector2 inputValue = value.Get<Vector2>();

        pitchInput = inputValue.y;
        rollInput += inputValue.x;
    }

    void FixedUpdate()
    {
        // 2. 조종면 회전 적용 (기체의 회전)
        ApplyRotation();

        // 3. 공기역학 물리 계산 (핵심)
        CalculateAerodynamics();
    }

    void ApplyRotation()
    {
        // Pitch (상하 기우뚱), Roll (좌우 회전) 제어
        float pitch = pitchInput * pitchSpeed * Time.fixedDeltaTime;
        float roll = -rollInput * rollSpeed * Time.fixedDeltaTime;

        // 기체의 로컬 축을 기준으로 회전 토크 부여
        rb.AddRelativeTorque(Vector3.right * pitch, ForceMode.Acceleration);
        rb.AddRelativeTorque(Vector3.forward * roll, ForceMode.Acceleration);
    }//

    void CalculateAerodynamics()
    {
        // 현재 전방 속도 계산 (기체가 바라보는 방향으로의 속도 성분 크기)
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        if (forwardSpeed < 0) forwardSpeed = 0; // 후진 시 양력 제외

        // 1. 양력(Lift) 계산: 전방 속도의 제곱에 비례하여 기체의 위쪽 방향으로 작용
        // 실제 비행기 물리 공식(L = 0.5 * p * v^2 * A * Cl)을 단순화
        float liftVelocityFactor = forwardSpeed * forwardSpeed;
        Vector3 liftForce = transform.up * (liftVelocityFactor * liftFactor);
        rb.AddForce(liftForce, ForceMode.Force);

        // 2. 항력(Drag) 계산: 현재 이동 속도의 반대 방향으로 가해지는 저항
        Vector3 dragForce = -rb.linearVelocity * (rb.linearVelocity.magnitude * dragFactor);
        rb.AddForce(dragForce, ForceMode.Force);

        // 3. 활공 지속력 유지를 위한 최소한의 전방 활공 보조력 (경사면 하강 효과 대체)
        // 코를 아래로 숙일수록(Pitch Down) 중력에 의해 전방 속도가 더 붙게 됩니다.
        float angleToGround = Vector3.Dot(transform.forward, Vector3.down);
        if (angleToGround > 0)
        {
            // 아래를 향할 때 전방 추진력 가속 (중력 에너지를 속도로 변환)
            rb.AddForce(transform.forward * angleToGround * forwardThrust, ForceMode.Force);
        }
    }
}