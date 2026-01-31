using UnityEngine;
using System.Collections;

public class Trampilla : MonoBehaviour
{
    [SerializeField] private float tiempoAbierta = 2f;
    [SerializeField] private float tiempoReactivacion = 3f;
    [SerializeField] private Vector3 rotacionAbierta = new Vector3(-55, 0, 0);

    private Quaternion rotacionOriginal;
    private bool estaActiva = false;
    private Collider miCollider; // Referencia al collider

    void Start()
    {
        rotacionOriginal = transform.localRotation;
        // Obtenemos el componente una sola vez al inicio por eficiencia
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

        // 1. Espera antes de caer
        yield return new WaitForSeconds(tiempoAbierta);

        // 2. ABRIR: Rotar y pasar a TRIGGER para que el player caiga limpio
        transform.localRotation = Quaternion.Euler(rotacionAbierta);
        miCollider.isTrigger = true;

        // 3. Esperar a que se reactive
        yield return new WaitForSeconds(tiempoReactivacion);

        // 4. CERRAR: Volver a la rotación y quitar el modo Trigger (Volver a ser sólido)
        transform.localRotation = rotacionOriginal;
        miCollider.isTrigger = false;

        estaActiva = false;
    }
}