using UnityEngine;

public class PlayerInteractionIndicator : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private GameObject indicadorE;

    [Header("Detección")]
    [SerializeField] private float radioDeteccion = 1.2f;
    [SerializeField] private LayerMask capaInteractuable;

    private bool hayInteractuableCerca;

    private void Awake()
    {
        if (indicadorE != null)
            indicadorE.SetActive(false);
    }

    private void Update()
    {
        DetectarInteractuable();
        ActualizarIndicador();
    }

    private void DetectarInteractuable()
    {
        hayInteractuableCerca = Physics.CheckSphere(
            transform.position,
            radioDeteccion,
            capaInteractuable,
            QueryTriggerInteraction.Collide
        );
    }

    private void ActualizarIndicador()
    {
        if (indicadorE == null) return;

        if (indicadorE.activeSelf != hayInteractuableCerca)
            indicadorE.SetActive(hayInteractuableCerca);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}
