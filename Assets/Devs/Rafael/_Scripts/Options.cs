using UnityEngine;
using UnityEngine.UI; // Necesario para controlar Sliders y Toggles

public class Options : MonoBehaviour
{
    [Header("Referencias UI")]
    public Slider sliderVolumen;
    public Toggle togglePantalla;

    void Start()
    {
        // Al iniciar, ajustamos la UI para que coincida con la configuración actual
        if (sliderVolumen != null)
            sliderVolumen.value = AudioListener.volume;

        if (togglePantalla != null)
            togglePantalla.isOn = Screen.fullScreen;
    }

    // Arrastra esto al evento "On Value Changed" del Slider
    public void CambiarVolumenGlobal(float volumen)
    {
        AudioListener.volume = volumen;
    }

    // Arrastra esto al evento "On Value Changed" del Toggle
    public void CambiarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
    }
}