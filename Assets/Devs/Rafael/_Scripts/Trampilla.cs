using UnityEngine;
using System.Collections;

public class Trampilla : MonoBehaviour
{
    [SerializeField] private float tiempoAbierta = 2f;
    [SerializeField] private float tiempoReactivacion = 3f;
    [SerializeField] private Vector3 rotacionAbierta = new Vector3(-55, 0, 0);

    private Quaternion rotacionOriginal;
    private bool estaActiva = false;
    private Collider miCollider;

    void Start()
    {
        rotacionOriginal = transform.localRotation;
        miCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !estaActiva)
        {
            StartCoroutine(SecuenciaTrampilla());
        }
    }

    IEnumerator SecuenciaTrampilla()
    {
        estaActiva = true;

        yield return new WaitForSeconds(tiempoAbierta);

        transform.localRotation = Quaternion.Euler(rotacionAbierta);
        miCollider.isTrigger = true;

        yield return new WaitForSeconds(tiempoReactivacion);

        transform.localRotation = rotacionOriginal;
        miCollider.isTrigger = false;

        estaActiva = false;
    }
}
