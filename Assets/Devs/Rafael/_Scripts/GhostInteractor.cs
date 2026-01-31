using UnityEngine;

public class GhostInteractor : MonoBehaviour
{
    [SerializeField] private float distanciaInteraccion = 4f;
    [SerializeField] private LayerMask capaBoss;
    [SerializeField] private KeyCode teclaAtaque = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(teclaAtaque))
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, distanciaInteraccion, capaBoss);
            foreach (var c in colls)
            {
                BossBulletSpawnerByPlayer boss = c.GetComponent<BossBulletSpawnerByPlayer>();
                if (boss != null) boss.RecibirDañoFantasma();
            }
        }
    }
}