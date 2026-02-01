using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BossBulletSpawnerByPlayer bossParent;
    public Vector3 direccionDisparo;
    public float velocidad;
    public float tiempoVida;
    public float danio; // Se asignará automáticamente desde el Boss

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        transform.position += direccionDisparo * velocidad * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) // Sin el "2D" y con "Collider" en vez de "Collider2D"
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null) health.RecibirDanio(danio);

            if (bossParent != null) bossParent.AplicarPenalizacionJugador();

            Destroy(gameObject);
        }
    }
}