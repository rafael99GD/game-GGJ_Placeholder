using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerControllerSimple : MonoBehaviour
{
    [Header("Ajustes")]
    public float velocidad = 14f;
    public float aceleracion = 90f;
    public float fuerzaSalto = 20f;
    public float rotacionSpeed = 20f;

    [Header("Físicas")]
    public float multCaida = 2.5f;    // Gravedad al caer
    public float multSaltoCorto = 8f; // Gravedad al soltar botón
    public LayerMask capaSuelo;

    Rigidbody _rb;
    CapsuleCollider _col;
    bool _tocaSuelo;
    bool _saltoPresionado;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        // Bloquea rotación y eje Z por código para asegurar 2.5D
        _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    void Update()
    {
        // Detectar input de salto (Buffer simple)
        if (Input.GetButtonDown("Jump") && _tocaSuelo)
        {
            _saltoPresionado = true;
        }
    }

    void FixedUpdate()
    {
        // Detectar Suelo (Posición exacta de los pies)
        Vector3 pies = _col.bounds.center;
        pies.y = _col.bounds.min.y + 0.1f;
        _tocaSuelo = Physics.CheckSphere(pies, 0.4f, capaSuelo);

        // Movimiento Horizontal
        Vector3 vel = _rb.linearVelocity;
        float inputX = Input.GetAxisRaw("Horizontal");

        vel.x = Mathf.MoveTowards(vel.x, inputX * velocidad, aceleracion * Time.fixedDeltaTime);

        // Ejecutar Salto
        if (_saltoPresionado)
        {
            vel.y = fuerzaSalto;
            _saltoPresionado = false;
            _tocaSuelo = false;
        }

        // Modificadores de Gravedad (Salto Variable)
        if (vel.y < 0) // Cayendo
        {
            vel.y += Physics.gravity.y * (multCaida - 1) * Time.fixedDeltaTime;
        }
        else if (vel.y > 0 && !Input.GetButton("Jump")) // Saltando pero soltó botón
        {
            vel.y += Physics.gravity.y * (multSaltoCorto - 1) * Time.fixedDeltaTime;
        }

        // Aplicar velocidad final
        _rb.linearVelocity = vel;

        // 5. Rotación Visual
        if (inputX != 0)
        {
            Quaternion rotDestino = Quaternion.LookRotation(Vector3.right * inputX);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotDestino, rotacionSpeed * Time.fixedDeltaTime);
        }
    }
}