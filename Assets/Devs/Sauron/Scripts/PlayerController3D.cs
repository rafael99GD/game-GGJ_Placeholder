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
    public float gravityMultiplier = 5f;
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
        _velocity = _rb.linearVelocity;

        CheckGround();
        HandleMovement();
        HandleJump();
        HandleGravity();

        _rb.linearVelocity = _velocity;

    }

    // --------------------------
    void CheckGround()
    {
        Vector3 footPos = _col.bounds.center;
        footPos.y = _col.bounds.min.y + 0.01f;

        float checkRadius = _col.radius * 0.05f;

        _grounded = Physics.CheckSphere(
            footPos,
            checkRadius,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );

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

        _velocity.z = 0f;
    }

    // --------------------------
    void HandleJump()
    {
        if (!_jumpPressed) return;

        if (_grounded)
        {
            _velocity.y = jumpPower;
            _grounded = false;
        }

        _jumpPressed = false;
    }

    // --------------------------
    void HandleGravity()
    {
        if (!_grounded)
        {
            _velocity.y += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;

            if (_velocity.y < -maxFallSpeed)
                _velocity.y = -maxFallSpeed;
        }
        else
        {
            if (_velocity.y < 0f)
                _velocity.y = 0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("ZonaProhibida"))
    {
        Debug.Log("No puedes pasar");
    }
}

}