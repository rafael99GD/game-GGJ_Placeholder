using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int velocidad;
    [SerializeField] private int tiempoVida = 8;

    GameObject player;
    float playerPos;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerPos = player.transform.position.x - this.gameObject.transform.position.x;
        StartCoroutine(Eliminar());
    }

    void Update()
    {
        if (playerPos > 0) this.transform.position = this.transform.position + (Vector3.right * velocidad * Time.deltaTime);
        else this.transform.position = this.transform.position + (Vector3.left * velocidad * Time.deltaTime);
    }

    IEnumerator Eliminar()
    {
        yield return new WaitForSeconds(tiempoVida);
        Destroy(this.gameObject);
    }
}
