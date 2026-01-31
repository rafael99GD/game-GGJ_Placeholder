using UnityEngine;

public class IndicadorInteraccion : MonoBehaviour
{
    [SerializeField] private GameObject bocadilloE;

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
        bocadilloE.SetActive(true);
}

private void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Player"))
        bocadilloE.SetActive(false);
}

}
