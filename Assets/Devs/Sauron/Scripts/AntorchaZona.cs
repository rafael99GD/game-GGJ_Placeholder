using UnityEngine;

public class AntorchaZona : MonoBehaviour
{
    [Header("Zona que bloquea al jugador")]
    [SerializeField] private Collider zonaProhibida;

    [Header("Visual del fuego")]
    [SerializeField] private GameObject fuegoVisual;

    private bool activa;

    public void Activar()
    {
        activa = true;
        Debug.Log("Antorcha ACTIVADA");

        if (zonaProhibida != null)
            zonaProhibida.enabled = true;

        if (fuegoVisual != null)
            fuegoVisual.SetActive(true);
    }

    public void Desactivar()
    {
        activa = false;
        Debug.Log("Antorcha DESACTIVADA");

        if (zonaProhibida != null)
            zonaProhibida.enabled = false;

        if (fuegoVisual != null)
            fuegoVisual.SetActive(false);
    }
}
