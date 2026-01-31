using UnityEngine;
using Interactions;

public class InteractWItems : MonoBehaviour
{
    [SerializeField] private Transform controladorInteractuar;
    [SerializeField] private Vector3 dimensionesCaja = new Vector3(1f, 1f, 1f);
    [SerializeField] private LayerMask capasInteractuables;

    PlayerInteractorState _state;

    void Awake()
    {
        _state = GetComponent<PlayerInteractorState>();
    }

    void Update()
    {
        if (_state != null && !_state.CanInteract) return;

        if (Input.GetButtonDown("Interactuar"))
        {
            Interactuar();
        }
    }

    void Interactuar()
    {
        Collider[] objetosTocados = Physics.OverlapBox(
            controladorInteractuar.position,
            dimensionesCaja * 0.5f,
            Quaternion.identity,
            capasInteractuables
        );

        foreach (Collider objeto in objetosTocados)
        {
            if (objeto.TryGetComponent(out IInteractuable interactuable))
            {
                interactuable.Interactuar();
                break;
            }
        }
    }

    void OnDrawGizmos()
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
