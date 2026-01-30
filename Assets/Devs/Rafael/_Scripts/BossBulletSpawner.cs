using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BossBulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float tiempoEntreBullets;
    [SerializeField] protected float offsetDistance;
    [SerializeField] float cantidadBalas;
    [SerializeField] float huecoBarrera;

    GameObject player;
    float playerPos;
    float puntoBajoY;
    float desplazamiento = 0.2f;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        StartCoroutine(SpawnBullet());
        playerPos = player.transform.position.x - this.gameObject.transform.position.x;

        //Este es el punto mas bajo que tiene la capsula para iniciar desde ahi la bala
        puntoBajoY = (float)(GetComponent<MeshRenderer>().bounds.min.y);
    }

    void Update()
    {
        playerPos = player.transform.position.x - this.gameObject.transform.position.x;
    }

    IEnumerator SpawnBullet()
    {
        while (true)
        {
            List<int> indicesSaltados = new List<int>();
            int primeraBala = (int) Random.Range(0 + huecoBarrera - 1, cantidadBalas);
            for(int i = 0; i < huecoBarrera; i++)
            {
                indicesSaltados.Add(primeraBala - i);
            }


            yield return new WaitForSeconds(tiempoEntreBullets);
            
            if (playerPos > 0)
            {
                for (int i = 0; i < cantidadBalas; i++)
                {
                    // Calculamos el desplazamiento de esta bala específica
                    if (i != 0) desplazamiento += 1;

                    // Creamos la posición final sumando ese desplazamiento al eje X (o Y si es vertical)
                    Vector3 posicionBarrera = new Vector3(this.transform.position.x + offsetDistance, puntoBajoY + desplazamiento, this.transform.position.z);

                    if(!indicesSaltados.Contains(i)) Instantiate(bullet, posicionBarrera, Quaternion.identity);
                }
            }
            else 
            {
                for (int i = 0; i < cantidadBalas; i++)
                {
                    // Calculamos el desplazamiento de esta bala específica
                    if (i != 0) desplazamiento += 1;

                    // Creamos la posición final sumando ese desplazamiento al eje X (o Y si es vertical)
                    Vector3 posicionBarrera = new Vector3(this.transform.position.x - offsetDistance, puntoBajoY + desplazamiento, this.transform.position.z);

                    if (!indicesSaltados.Contains(i)) Instantiate(bullet, posicionBarrera, Quaternion.identity);
                }
            }

            desplazamiento = 0.2f;
            indicesSaltados = new List<int>();

        }
    }
}
