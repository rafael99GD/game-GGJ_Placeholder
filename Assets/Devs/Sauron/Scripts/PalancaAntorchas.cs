using UnityEngine;
using Interactions;

public class PalancaAntorchas : MonoBehaviour, IInteractuable
{
    [Header("Antorchas a activar")]
    [SerializeField] private AntorchaZona[] antorchasOn;

    [Header("Antorchas a desactivar")]
    [SerializeField] private AntorchaZona[] antorchasOff;

    private bool activada;

    public void Interactuar()
    {
        activada = !activada;

        if (activada)
        {
            foreach (var a in antorchasOn)
                a.Activar();

            foreach (var a in antorchasOff)
                a.Desactivar();
        }
        else
        {
            foreach (var a in antorchasOn)
                a.Desactivar();

            foreach (var a in antorchasOff)
                a.Activar();
        }
    }
}
