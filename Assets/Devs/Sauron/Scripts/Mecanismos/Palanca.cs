using UnityEngine;
using Interactions;

public class Palanca : MonoBehaviour, IInteractuable
{
    [Header("Referencia visual")]
    [SerializeField] private Transform arm;   // Subcomponente "Arm"

    [Header("Ángulos en X")]
    [SerializeField] private float anguloReposo = 25f;
    [SerializeField] private float anguloActivado = -25f;

    [Header("Animación")]
    [SerializeField] private float duracionRotacion = 0.15f;

    private bool activada;
    private bool enRotacion;

    private Quaternion rotReposo;
    private Quaternion rotActivada;

    private void Awake()
    {
        if (arm == null)
        {
            Debug.LogError("Palanca: referencia 'arm' no asignada");
            return;
        }

        // Guardamos las dos rotaciones posibles del Arm
        rotReposo = Quaternion.Euler(anguloReposo, 0f, 0f);
        rotActivada = Quaternion.Euler(anguloActivado, 0f, 0f);

        // Aseguramos estado inicial
        arm.localRotation = rotReposo;
    }

    public void Interactuar()
    {
        if (enRotacion || arm == null) return;

        activada = !activada;

        StopAllCoroutines();
        StartCoroutine(RotarArm(activada ? rotActivada : rotReposo));
    }

    private System.Collections.IEnumerator RotarArm(Quaternion destino)
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
}
