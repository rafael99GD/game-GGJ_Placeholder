using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider sliderVolumen;

    void OnEnable()
    {
        // 1. Recuperar el valor guardado (0.5 por defecto)
        float valorGuardado = PlayerPrefs.GetFloat("VolumenGlobal", 0.5f);

        // 2. Aplicarlo al AudioListener (por si acaso)
        AudioListener.volume = valorGuardado;

        // 3. Aplicarlo al Slider SIN disparar eventos accidentales
        if (sliderVolumen != null)
        {
            sliderVolumen.value = valorGuardado;
        }
    }

    public void GuardarVolumen(float valor)
    {
        // Solo guardamos si el valor es distinto de lo que ya tenemos
        // Esto evita que el Slider sobrescriba con 0 al inicializarse
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("VolumenGlobal", valor);
        PlayerPrefs.Save(); // Forzamos el guardado en el disco
    }
}