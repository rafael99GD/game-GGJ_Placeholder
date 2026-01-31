using UnityEngine;
using Interactions;

public class Palanca : MonoBehaviour, IInteractuable
{
    [SerializeField] private bool activada;
    [SerializeField] private Animator animator;

    public void Interactuar()
    {
        activada = !activada;

        if (animator != null)
            animator.SetBool("Activada", activada);

        Debug.Log("Palanca: " + (activada ? "ON" : "OFF"));
    }
}