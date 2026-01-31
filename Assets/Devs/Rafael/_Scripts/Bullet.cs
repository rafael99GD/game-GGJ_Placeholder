using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Paraemtros de la Bala")]
    [HideInInspector] public int velocidad;
    [HideInInspector] public int tiempoVida = 8;
    [HideInInspector] public Vector3 direccionDisparo;

    private void Start()
    {
        StartCoroutine(EliminarBala());
    }

    void Update()
    {
        transform.position += direccionDisparo * velocidad * Time.deltaTime;
    }

    IEnumerator EliminarBala()
    {
        yield return new WaitForSeconds(tiempoVida);
        Destroy(this.gameObject);
    }

}
