using UnityEngine;
using Interactions;

public class TomaCorriente : MonoBehaviour, IInteractuable
{
    private bool tieneEnergia = false;

    public void Interactuar()
    {
        tieneEnergia = !tieneEnergia;

        Debug.Log(tieneEnergia 
            ? "Corriente activada" 
            : "Corriente desactivada");
    }
}
