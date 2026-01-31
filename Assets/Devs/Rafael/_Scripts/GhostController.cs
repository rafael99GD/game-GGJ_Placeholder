using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GhostController : MonoBehaviour
{
    public float speed = 10f;
    public bool isActive = false;

    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        // Asignamos la capa "Ghost" por código para evitar errores manuales
        gameObject.layer = LayerMask.NameToLayer("Ghost");
    }

    void FixedUpdate()
    {
        if (!isActive)
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, v, 0).normalized;
        _rb.linearVelocity = direction * speed;
    }
}