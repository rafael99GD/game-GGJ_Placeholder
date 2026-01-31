using UnityEngine;
using System.Collections; // Necesario para las Corrutinas

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 14f;
    public float acceleration = 120f;
    public float airDeceleration = 60f;

    [Header("Jump")]
    public float jumpPower = 20f;
    public float doubleJumpPower = 15f;
    [Range(0f, 1f)] public float jumpCutMultiplier = 0.3f;
    private int _jumpsRemaining;

    [Header("Ghost Jump Effect")]
    [SerializeField] private GameObject ghostVisualObject; // Arrastra aquí el objeto con el sprite del fantasma
    [SerializeField] private float ghostDuration = 0.5f;   // Cuánto tiempo se queda visible
    [SerializeField] private Vector3 ghostOffset = new Vector3(0, -1f, 0); // Posición debajo del player

    [Header("Gravity")]
    public float gravityMultiplier = 5f;
    public float fallMultiplier = 1.8f;
    public float maxFallSpeed = 50f;

    [Header("Detection")]
    public LayerMask groundLayer;

    Rigidbody _rb;
    CapsuleCollider _col;
    Vector3 _velocity;
    bool _grounded;
    bool _jumpPressed;
    bool _isJumping;

    public bool isControlEnabled = true;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        _rb.useGravity = false;

        // Aseguramos que el fantasma visual esté apagado al inicio
        if (ghostVisualObject != null) ghostVisualObject.SetActive(false);
    }

    void Update()
    {
        if (!isControlEnabled) return;

        if (Input.GetButtonDown("Jump"))
        {
            if (_grounded || _jumpsRemaining > 0) _jumpPressed = true;
        }

        if (Input.GetButtonUp("Jump") && _rb.linearVelocity.y > 0 && _isJumping)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _rb.linearVelocity.y * jumpCutMultiplier, _rb.linearVelocity.z);
            _isJumping = false;
        }
    }

    void FixedUpdate()
    {
        _velocity = _rb.linearVelocity;
        CheckGround();

        if (!isControlEnabled)
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, acceleration * Time.fixedDeltaTime);
            HandleGravity();
            _rb.linearVelocity = _velocity;
            return;
        }

        HandleMovement();
        HandleJump();
        HandleGravity();
        _rb.linearVelocity = _velocity;
    }

    void CheckGround()
    {
        Vector3 footPos = _col.bounds.center;
        footPos.y = _col.bounds.min.y + 0.01f;
        _grounded = Physics.CheckSphere(footPos, _col.radius * 0.9f, groundLayer, QueryTriggerInteraction.Ignore);

        if (_grounded && _velocity.y <= 0)
        {
            _isJumping = false;
            _jumpsRemaining = 1;
        }
    }

    void HandleMovement()
    {
        float input = Input.GetAxisRaw("Horizontal");
        _velocity.x = Mathf.MoveTowards(_velocity.x, input * maxSpeed, (_grounded ? acceleration : airDeceleration) * Time.fixedDeltaTime);
    }

    void HandleJump()
    {
        if (_jumpPressed)
        {
            if (_grounded)
            {
                _velocity.y = jumpPower;
            }
            else
            {
                // DOBLE SALTO
                _velocity.y = doubleJumpPower;
                _jumpsRemaining--;

                // ACTIVAR EFECTO VISUAL
                if (ghostVisualObject != null)
                {
                    StopAllCoroutines(); // Por si saltas muy rápido
                    StartCoroutine(ShowGhostEffect());
                }
            }

            _isJumping = true;
            _jumpPressed = false;
        }
    }

    IEnumerator ShowGhostEffect()
    {
        // Posicionamos el fantasma relativo al jugador en ese momento
        ghostVisualObject.transform.position = transform.position + ghostOffset;
        ghostVisualObject.SetActive(true);

        yield return new WaitForSeconds(ghostDuration);

        ghostVisualObject.SetActive(false);
    }

    void HandleGravity()
    {
        if (!_grounded)
        {
            float mult = (_velocity.y < 0) ? gravityMultiplier * fallMultiplier : gravityMultiplier;
            _velocity.y += Physics.gravity.y * mult * Time.fixedDeltaTime;
            if (_velocity.y < -maxFallSpeed) _velocity.y = -maxFallSpeed;
        }
        else if (_velocity.y < 0) _velocity.y = -0.1f;
    }
}