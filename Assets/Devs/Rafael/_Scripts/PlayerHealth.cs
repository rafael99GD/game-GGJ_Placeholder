using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    [SerializeField] private float vidaMaxima = 100f;
    private float vidaActual;

    [Header("Interfaz de Usuario")]
    [SerializeField] private Slider barraVida;
    [SerializeField] private GameObject contenedorBarra;

    void Start()
    {
        vidaActual = vidaMaxima;

        if (barraVida != null)
        {
            barraVida.maxValue = vidaMaxima;
            barraVida.value = vidaActual;
        }

        ActualizarVisibilidadBarra();
    }

    public void RecibirDanio(float cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        ActualizarUI();

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    public void CurarVidaCompleta()
    {
        vidaActual = vidaMaxima;
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        if (barraVida != null) barraVida.value = vidaActual;
        ActualizarVisibilidadBarra();
    }

    private void ActualizarVisibilidadBarra()
    {
        if (contenedorBarra != null)
        {
            contenedorBarra.SetActive(vidaActual < vidaMaxima);
        }
    }

    private void Morir()
    {
        Debug.Log("Jugador derrotado. Reiniciando batalla...");

        string nombreEscenaActual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nombreEscenaActual);
    }
}
