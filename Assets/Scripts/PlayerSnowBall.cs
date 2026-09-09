using UnityEngine;

public class PlayerSnowBall : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(0, -0.1f, 0.35f);
    private float growFactor = 0.05f;
    private float maxScale = 5.0f;
    private float baseForwardOffset = 0.35f;
    private float baseHeightOffset = -0.1f;
    private int cubesPerLevel = 5;
    [SerializeField] private int snowCount = 0;
    private Vector3 defaultScale;
    private Rigidbody rb;
    public bool isConnected = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        defaultScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
        isConnected = true;
    }

    void Update()
    {
        if (!isConnected) return;
        float currentRadius = transform.localScale.x / 2f;
        float dynamicOffsetZ = 0.25f + currentRadius + baseForwardOffset;
        float dynamicOffsetY = baseHeightOffset + (currentRadius - 0.25f);
        Vector3 dynamicOffset = player.transform.forward * dynamicOffsetZ + player.transform.up * dynamicOffsetY;
        transform.position = player.transform.position + dynamicOffset;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SnowCube"))
        {
            snowCount++;
            if (snowCount >= cubesPerLevel)
            {
                snowCount = 0;
                IncreaseScale();
            }
        }
    }

    public void SetConnectBool(bool _isConnected)
    {
        isConnected = _isConnected;
    }

    private void IncreaseScale()
    {
        Vector3 currentScale = transform.localScale;
        if (currentScale.x < maxScale)
        {
            Vector3 newScale = currentScale + new Vector3(growFactor, growFactor, growFactor);

            if (newScale.x > maxScale)
            {
                newScale = new Vector3(maxScale, maxScale, maxScale);
            }

            transform.localScale = newScale;
        }
        // snowCount++;
        // if (transform.localScale.magnitude >= defaultScale.magnitude * 8)
        // {
        //     return;
        // }
        // Vector3 scale = transform.localScale * (50 + snowCount) / 50;
        // offset.z = 0.6f + scale.z / 2;
        // transform.localScale = scale;

    }
}
