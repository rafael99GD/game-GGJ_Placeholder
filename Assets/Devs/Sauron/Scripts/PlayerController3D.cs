using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController3D : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 14f;
    public float acceleration = 120f;
    public float groundDeceleration = 140f;
    public float airDeceleration = 60f;

    [Header("Jump")]
    public float jumpPower = 20f;

    [Header("Gravity")]
    public float gravityMultiplier = 5f; // cuánto más grande, más rápido cae
    public float maxFallSpeed = 50f;

    [Header("Ground Detection")]
    public LayerMask groundLayer;

    Rigidbody _rb;
    CapsuleCollider _col;

    Vector3 _velocity;
    bool _grounded;
    bool _jumpPressed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("SALTO DETECTADO");
            _jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        // Partimos de la velocidad actual
        _velocity = _rb.linearVelocity;

        CheckGround();
        HandleMovement();
        HandleJump();
        HandleGravity();

        // Aplica la velocidad final
        _rb.linearVelocity = _velocity;

        // Debug para comprobar grounded y velocidad
        Debug.Log("Grounded: " + _grounded + " | Velocity Y: " + _velocity.y);
    }

    // --------------------------
    void CheckGround()
    {
        // Posición del pie
        Vector3 footPos = _col.bounds.center;
        footPos.y = _col.bounds.min.y + 0.01f;

        float checkRadius = _col.radius * 0.05f; // ligeramente más pequeño que la base del collider

        _grounded = Physics.CheckSphere(
            footPos,
            checkRadius,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );

        // Debug visual en Scene
        Debug.DrawRay(footPos, Vector3.down * 0.5f, _grounded ? Color.green : Color.red);
    }

    // --------------------------
    void HandleMovement()
    {
        float input = Input.GetAxisRaw("Horizontal");

        float targetSpeed = input * maxSpeed;
        float accel = _grounded ? acceleration : airDeceleration;

        _velocity.x = Mathf.MoveTowards(
            _rb.linearVelocity.x,
            targetSpeed,
            accel * Time.fixedDeltaTime
        );

        _velocity.z = 0f; // bloquea eje Z
    }

    // --------------------------
    void HandleJump()
    {
        if (!_jumpPressed) return;

        if (_grounded)
        {
            _velocity.y = jumpPower;
            _grounded = false; // para evitar que el suelo bloquee inmediatamente
        }

        _jumpPressed = false;
    }

    // --------------------------
    void HandleGravity()
    {
        if (!_grounded)
        {
            // Caída directa y contundente
            _velocity.y += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;

            if (_velocity.y < -maxFallSpeed)
                _velocity.y = -maxFallSpeed;
        }
        else
        {
            // Si tocamos suelo, bloquea velocidad descendente
            if (_velocity.y < 0f)
                _velocity.y = 0f;
        }
    }
}
