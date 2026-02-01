using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GhostController : MonoBehaviour
{
    public float speed = 10f;
    public bool isActive;

    Rigidbody rb;
    SpriteRenderer rend;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Buscamos el renderer en el objeto o en sus hijos (donde esté el sprite)
        rend = GetComponentInChildren<SpriteRenderer>();

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        gameObject.layer = LayerMask.NameToLayer("Ghost");
    }

    void FixedUpdate()
    {
        if (!isActive)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(h, v, 0).normalized;
        rb.linearVelocity = move * speed;

        // --- LÓGICA DE FLIP ---
        if (h != 0 && rend != null)
        {
            // Si h es mayor que 0 (derecha), flipX = true
            // Si h es menor que 0 (izquierda), flipX = false
            // (Ajusta los valores true/false si tu sprite original mira al revés)
            rend.flipX = (h > 0);
        }
    }
}