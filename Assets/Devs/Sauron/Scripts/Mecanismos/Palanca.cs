using UnityEngine;
using System.Collections;
using Interactions;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class Palanca : MonoBehaviour, IInteractuable
{
    [Header("Referencia visual Palanca")]
    [SerializeField] private Transform arm;
    [SerializeField] private float anguloReposo = 25f;
    [SerializeField] private float anguloActivado = -25f;
    [SerializeField] private float duracionRotacion = 0.15f;

    [Header("Lógica de la Verja")]
    [SerializeField] private Transform verja;
    [SerializeField] private Transform puntoDestino;
    [SerializeField] private float velocidadVerja = 2f;

    private bool activada;
    private bool enRotacion;

    private Quaternion rotReposo;
    private Quaternion rotActivada;
    private Vector3 posicionOriginalVerja;
    private Coroutine corrutinaVerja;

    private void Awake()
    {
        if (arm == null)
        {
            Debug.LogError("Palanca: referencia 'arm' no asignada");
            return;
        }

        rotReposo = Quaternion.Euler(anguloReposo, 0f, 0f);
        rotActivada = Quaternion.Euler(anguloActivado, 0f, 0f);
        arm.localRotation = rotReposo;

        if (verja != null)
        {
            posicionOriginalVerja = verja.position;
        }
    }

    public void Interactuar()
    {
        if (enRotacion || arm == null) return;

        activada = !activada;

        StopCoroutine("RotarArm");
        StartCoroutine(RotarArm(activada ? rotActivada : rotReposo));

        if (verja != null && puntoDestino != null)
        {
            if (corrutinaVerja != null) StopCoroutine(corrutinaVerja);

            Vector3 destino = activada ? puntoDestino.position : posicionOriginalVerja;
            corrutinaVerja = StartCoroutine(MoverVerja(destino));
        }
    }

    private IEnumerator RotarArm(Quaternion destino)
    {
        enRotacion = true;
        Quaternion inicio = arm.localRotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duracionRotacion;
            arm.localRotation = Quaternion.Slerp(inicio, destino, t);
            yield return null;
        }

        arm.localRotation = destino;
        enRotacion = false;
    }

    private IEnumerator MoverVerja(Vector3 destinoFinal)
    {
        while (Vector3.Distance(verja.position, destinoFinal) > 0.01f)
        {
            verja.position = Vector3.MoveTowards(
                verja.position,
                destinoFinal,
                velocidadVerja * Time.deltaTime
            );
            yield return null;
        }

        verja.position = destinoFinal;

        // NUEVA LÓGICA: Si la verja ha llegado al destino y la palanca está activada
        if (activada && verja.position == puntoDestino.position)
        {
            CargarSiguienteEscena();
        }
    }

    private void CargarSiguienteEscena()
    {
        Debug.Log("Verja en posición. Cargando Game_2...");
        SceneManager.LoadScene("Game_2");
    }
}