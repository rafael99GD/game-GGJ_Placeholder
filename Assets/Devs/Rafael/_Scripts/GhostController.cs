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

        if (h != 0 && rend != null)
        {
            rend.flipX = (h > 0);
        }
    }
}
