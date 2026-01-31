using UnityEngine;
using Interactions;

public class InteractWItems : MonoBehaviour
{
    [SerializeField] private Transform controladorInteractuar;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private LayerMask capasInteractuables;

    private void Update()
    {
        if (Input.GetButtonDown("Interactuar"))
        {
            Interactuar();
        }
    }

    private void Interactuar()
    {
        Debug.Log("Intentando interactuar");

        Collider[] objetosTocados = Physics.OverlapBox(
            controladorInteractuar.position,
            dimensionesCaja / 2,
            Quaternion.identity,
            capasInteractuables
        );

        Debug.Log("Detectados: " + objetosTocados.Length);

        foreach (Collider objeto in objetosTocados)
        {
            Debug.Log("Tocado: " + objeto.name);

            if (objeto.TryGetComponent(out IInteractuable interactuable))
            {
                Debug.Log("Interactuable encontrado");
                interactuable.Interactuar();
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (controladorInteractuar == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS(
            controladorInteractuar.position,
            Quaternion.identity,
            Vector3.one
        );
        Gizmos.DrawWireCube(Vector3.zero, dimensionesCaja);
    }
}
