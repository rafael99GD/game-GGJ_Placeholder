using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [Header("Configuracion puntos de la plataforma")]
    [SerializeField] private GameObject puntoA;
    [SerializeField] private GameObject puntoB;
    [SerializeField] private float velocidad;

    private Vector3 destinoActual;

    void Start()
    {
        destinoActual = puntoA.transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinoActual, velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, destinoActual) < 0.1f)
        {
            if (destinoActual == puntoA.transform.position) destinoActual = puntoB.transform.position;
            else destinoActual = puntoA.transform.position;
        }
    }
}