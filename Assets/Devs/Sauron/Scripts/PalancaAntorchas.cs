using UnityEngine;
using Interactions;

public class PalancaAntorchas : MonoBehaviour, IInteractuable
{
    [SerializeField] private AntorchaZona[] antorchasAActivar;

    private bool activada;

    public void Interactuar()
    {
        activada = !activada;

        Debug.Log("Palanca usada. Estado: " + activada);

        if (activada)
        {
            foreach (var antorcha in antorchasAActivar)
            {
                if (antorcha != null)
                    antorcha.Activar();
            }
        }
        else
        {
            foreach (var antorcha in antorchasAActivar)
            {
                if (antorcha != null)
                    antorcha.Desactivar();
            }
        }
    }
}
