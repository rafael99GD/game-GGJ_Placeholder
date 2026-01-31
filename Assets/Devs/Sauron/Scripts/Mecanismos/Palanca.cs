using UnityEngine;
using Interactions;

public class Palanca : MonoBehaviour, IInteractuable
{
    [SerializeField] private bool activada;
    [SerializeField] private Animator animator;

    public bool Activada => activada;

    public void Interactuar()
    {
        activada = !activada;

        Debug.Log($"Palanca {name}: {(activada ? "ON" : "OFF")}");

        if (animator != null)
            animator.SetBool("Activada", activada);
    }
}
