using UnityEngine;

public class AntorchaZona : MonoBehaviour
{
    [SerializeField] private Collider2D zonaProhibida;
    [SerializeField] private GameObject fuegoVisual;

    private bool activa;

    public void Activar()
    {
        activa = true;

        if (zonaProhibida != null)
            zonaProhibida.enabled = true;

        if (fuegoVisual != null)
            fuegoVisual.SetActive(true);
    }

    public void Desactivar()
    {
        activa = false;

        if (zonaProhibida != null)
            zonaProhibida.enabled = false;

        if (fuegoVisual != null)
            fuegoVisual.SetActive(false);
    }
}
