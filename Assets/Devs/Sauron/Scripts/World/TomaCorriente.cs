using UnityEngine;
using Interactions;

public class TomaCorriente : MonoBehaviour, IInteractuable
{
    public bool TieneEnergia { get; private set; }

    public void Interactuar()
    {
        TieneEnergia = !TieneEnergia;

        Debug.Log(
            TieneEnergia
                ? $"Corriente ACTIVADA en {name}"
                : $"Corriente DESACTIVADA en {name}"
        );
    }
}
