using UnityEngine;

public class CreditsAutoExit : MonoBehaviour
{
    [Header("Duración de los créditos")]
    [SerializeField] private float segundosHastaSalir = 8f;

    private void Start()
    {
        Invoke(nameof(SalirDelJuego), segundosHastaSalir);
    }

    private void SalirDelJuego()
    {
        Debug.Log("Créditos finalizados. Cerrando el juego.");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
