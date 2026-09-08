using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offset = new Vector3(0, 1, -3);

    private float smoothTime = 0.05f;

    private Vector3 _currentVelocity;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        ApplyViewImmediate();
    }

    private void ApplyViewImmediate()
    {
        transform.position = target.TransformPoint(offset);
        transform.LookAt(target.transform.position);
        _currentVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 targetWorldPos = target.TransformPoint(offset);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetWorldPos,
            ref _currentVelocity,
            smoothTime
        );

        Vector3 lookTarget = target.transform.position;
        transform.LookAt(lookTarget);
    }
}