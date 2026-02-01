using UnityEngine;

public class AntorchaZona : MonoBehaviour
{
    [Header("Zona que bloquea al jugador")]
    [SerializeField] private Collider zonaProhibida;

    [Header("Visual del fuego")]
    [SerializeField] private GameObject fuegoVisual;

    public bool Activa { get; private set; }

    private void Awake()
    {
        Desactivar();
    }

    public void Activar()
    {
        Activa = true;
        Debug.Log($"Antorcha {name} ACTIVADA");

        if (zonaProhibida != null)
            zonaProhibida.enabled = true;

        if (fuegoVisual != null)
            fuegoVisual.SetActive(true);
    }

    public void Desactivar()
    {
        Activa = false;
        Debug.Log($"Antorcha {name} DESACTIVADA");

        if (zonaProhibida != null)
            zonaProhibida.enabled = false;

        if (fuegoVisual != null)
            fuegoVisual.SetActive(false);
    }
}
