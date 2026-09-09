using System.Collections;
using UnityEngine;

public class SnowCube : MonoBehaviour
{
    public Renderer rend;
    public Collider col;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerSnowBall"))
        {
            DisableSelf();
        }
    }

    public void EnableSelf()
    {
        rend.enabled = true;
        col.enabled = true;
    }

    public void DisableSelf()
    {
        rend.enabled = false;
        col.enabled = false;
    }
}
