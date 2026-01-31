using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Parametros de la Bala")]
    [HideInInspector] public int velocidad;
    [HideInInspector] public int tiempoVida = 8;
    [HideInInspector] public Vector3 direccionDisparo;

    // Referencia al spawner para avisarle del golpe
    [HideInInspector] public BossBulletSpawnerByPlayer bossParent;

    private void Start()
    {
        StartCoroutine(EliminarBala());
    }

    void Update()
    {
        transform.position += direccionDisparo * velocidad * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (bossParent != null)
            {
                // Ahora llamamos a la función que usa el incremento de la fase actual
                bossParent.AplicarPenalizacionJugador();
            }
            Destroy(this.gameObject);
        }
    }

    IEnumerator EliminarBala()
    {
        yield return new WaitForSeconds(tiempoVida);
        Destroy(this.gameObject);
    }
}