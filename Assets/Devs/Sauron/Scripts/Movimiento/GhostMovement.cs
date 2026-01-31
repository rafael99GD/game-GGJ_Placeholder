using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    [Header("Ghost Settings")]
    public float speed = 15f;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, v, 0).normalized;

        // Movimiento directo sin gravedad (la gravedad se apaga en el Switcher)
        _rb.linearVelocity = dir * speed;
    }
}