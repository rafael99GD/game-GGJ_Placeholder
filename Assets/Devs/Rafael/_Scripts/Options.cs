using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider sliderVolumen;

    void OnEnable()
    {
        float valorGuardado = PlayerPrefs.GetFloat("VolumenGlobal", 0.5f);
        AudioListener.volume = valorGuardado;

        if (sliderVolumen != null)
        {
            sliderVolumen.value = valorGuardado;
        }
    }

    public void GuardarVolumen(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("VolumenGlobal", valor);
        PlayerPrefs.Save();
    }
}
