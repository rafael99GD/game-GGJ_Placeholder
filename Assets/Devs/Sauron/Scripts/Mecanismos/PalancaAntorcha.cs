using UnityEngine;
using Interactions;

public class PalancaAntorchas : MonoBehaviour, IInteractuable
{
    [SerializeField] private AntorchaZona[] antorchasAControlar;

    private bool activada;

    public void Interactuar()
    {
        activada = !activada;
        Debug.Log($"PalancaAntorchas {name} → {activada}");

        foreach (var antorcha in antorchasAControlar)
        {
            if (antorcha == null) continue;

            if (activada)
                antorcha.Activar();
            else
                antorcha.Desactivar();
        }
    }
}
