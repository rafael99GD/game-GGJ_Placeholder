using UnityEngine;
using Interactions;

public class DoorTeleportSimple : MonoBehaviour, IInteractuable
{
    [Header("Referencias a mover")]
    [SerializeField] private Transform persona;    // Jugador 1
    [SerializeField] private Transform fantasma;   // Jugador 2
    [SerializeField] private Transform camara;     // Cámara única

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
        // Teleport Persona
        if (persona != null && personaDestino != null)
            persona.position = personaDestino.position;

        // Teleport Fantasma (aunque luego se desactive)
        if (fantasma != null && fantasmaDestino != null)
            fantasma.position = fantasmaDestino.position;

        // Mover cámara
        if (camara != null && camaraDestino != null)
            camara.position = camaraDestino.position;

        // 🔑 APUNTE EXTRA: Fantasma queda inactivo tras cruzar la puerta
        if (fantasma != null && fantasma.gameObject.activeSelf)
            fantasma.gameObject.SetActive(false);

        Debug.Log($"Teleport realizado por {name}. Fantasma desactivado.");
    }
}
