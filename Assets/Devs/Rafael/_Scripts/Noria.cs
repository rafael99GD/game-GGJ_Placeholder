using UnityEngine;

public class Noria : MonoBehaviour
{
    [Header("Plataformas")]
    [SerializeField] private GameObject[] plataformas;

    [Header("Configuracion rotacion noria")]
    [SerializeField] private float velocidadRotacion;
    [SerializeField] private bool izquierda;

    private float direccionFinal;

    // Update is called once per frame
    void Update()
    {
        direccionFinal = izquierda ? 1 : -1;

        transform.Rotate(0, 0, velocidadRotacion * direccionFinal * Time.deltaTime);

        for(int i = 0; i < plataformas.Length; i++) { plataformas[i].transform.rotation = Quaternion.identity; }
    }
}
