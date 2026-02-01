using UnityEngine;

public class SimpleSoundManager : MonoBehaviour
{
    public AudioClip musicaDeEstaEscena;
    private AudioSource fuente;

    void Awake()
    {
        // FORZAMOS el volumen maestro al despertar, por si acaso
        float volGuardado = PlayerPrefs.GetFloat("VolumenGlobal", 0.5f);
        AudioListener.volume = volGuardado;
        Debug.Log("AudioListener inicializado en: " + volGuardado);
    }

    void Start()
    {
        if (musicaDeEstaEscena == null)
        {
            Debug.LogError("¡No hay clip de audio en el SoundManager de esta escena!");
            return;
        }

        fuente = gameObject.AddComponent<AudioSource>();
        fuente.clip = musicaDeEstaEscena;
        fuente.loop = true;

        // Ponemos el volumen de la fuente al máximo (1) 
        // para que solo mande el AudioListener global
        fuente.volume = 1f;

        fuente.Play();
        Debug.Log("Reproduciendo música: " + musicaDeEstaEscena.name);
    }
}