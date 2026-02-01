using UnityEngine;
using Interactions;

public class DoorTeleportSimple : MonoBehaviour, IInteractuable
{
    [Header("Referencias a mover")]
    [SerializeField] private Transform persona;
    [SerializeField] private Transform fantasma;
    [SerializeField] private Transform camara;

    [Header("Destino del siguiente escenario")]
    [SerializeField] private Transform personaDestino;
    [SerializeField] private Transform fantasmaDestino;
    [SerializeField] private Transform camaraDestino;

    public void Interactuar()
    {
        Teleportar();
    }

    private void Teleportar()
    {
        if (persona != null && personaDestino != null)
            persona.position = personaDestino.position;

        if (fantasma != null && fantasmaDestino != null)
            fantasma.position = fantasmaDestino.position;

        if (camara != null && camaraDestino != null)
            camara.position = camaraDestino.position;

        if (fantasma != null && fantasma.gameObject.activeSelf)
            fantasma.gameObject.SetActive(false);

        Debug.Log($"Teleport realizado por {name}. Fantasma desactivado.");
    }
}
