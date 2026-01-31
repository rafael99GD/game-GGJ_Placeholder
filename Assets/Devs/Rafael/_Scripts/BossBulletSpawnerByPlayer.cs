using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BossBulletSpawnerByPlayer : MonoBehaviour
{
    [Header("Parametros de activacion del Boss")]
    [SerializeField] private bool activado;
    [SerializeField] private GameObject bala;

    [Header("Direccion del disparo")]
    [SerializeField] private bool horizontal;
    [SerializeField] private bool vertical;
    [SerializeField] private bool diagonal;
    int eleccionDisparo = -1;

    [Header("Parametros de generacion de barreras")]
    [SerializeField] private int tiempoEntreBarreras;
    private bool puedeDisparar;

    [Header("Parametros de configuracion de las barreras Horizontal")]
    [SerializeField] private float offsetBalaHorizontal_H;
    [SerializeField] private float numeroDeBalas_H;
    [SerializeField] private float separacionEntreBalas_H;
    [SerializeField] private float huecoBarrera_H;

    [Header("Parametros de configuracion de las balas Horizontal")]
    [SerializeField] private int velocidadBala_H;
    [SerializeField] public int tiempoVidaBala_H = 8;

    [Header("Parametros de configuracion de las barreras Vertical")]
    [SerializeField] private float offsetBalaHorizontal_V;
    [SerializeField] private float offsetBalaVertical_V;
    [SerializeField] private float numeroDeBalas_V;
    [SerializeField] private float separacionEntreBalas_V;
    [SerializeField] private float huecoBarrera_V;

    [Header("Parametros de configuracion de las balas Vertical")]
    [SerializeField] private int velocidadBala_V;
    [SerializeField] public int tiempoVidaBala_V = 8;

    [Header("Parametros de configuracion de las barreras Diagonal")]
    [SerializeField] private float offsetBalaHorizontal_D;
    [SerializeField] private float offsetBalaVertical_D;
    [SerializeField] private float numeroDeBalas_D;
    [SerializeField] private float separacionEntreBalas_D;
    [SerializeField] private float huecoBarrera_D;

    [Header("Parametros de configuracion de las balas Diagonal")]
    [SerializeField] private int velocidadBala_D;
    [SerializeField] public int tiempoVidaBala_D = 8;

    //Parametros necesarios
    GameObject player;
    float playerPos;

    //Direccion que tomara la bala
    public static Vector3 direccionFinal;

    private void Start()
    {
        //Al ser instanciado puede disparar desde un primer momento
        puedeDisparar = true;

        //Obtenemos el Gameobject del player
        player = GameObject.FindWithTag("Player");

        if (player == null) return;

        //Obtenemos en que direccion esta el player (Si > 0 = Derecha, Si < 0 = Izquierda)
        playerPos = player.transform.position.x - this.gameObject.transform.position.x;
    }

    private void Update()
    {
        if (!activado) return;

        if(puedeDisparar)
        {
            playerPos = player.transform.position.x - this.gameObject.transform.position.x;

            puedeDisparar = false;

            //Aqui seleccionamos aleatoriamente que disparo sera el siguiente (0 = Horizontal, 1 = Vertical, 2 = Diagonal)
            List<int> opcionesValidas = new List<int>();

            if (horizontal) opcionesValidas.Add(0);
            if (vertical) opcionesValidas.Add(1);
            if (diagonal) opcionesValidas.Add(2);

            if (opcionesValidas.Count > 0) 
            {
                int indiceRandom = Random.Range(0, opcionesValidas.Count);
                eleccionDisparo = opcionesValidas[indiceRandom];
            } else
            {
                eleccionDisparo = -1;
            }

            Debug.Log("Eleccion Disparo: " + eleccionDisparo);

            switch (eleccionDisparo)
            {
                case 0:
                    Debug.Log("Disparo Horizontal efectuado");

                    //Declaramos la direccion que tomara la bala
                    if (playerPos < 0) direccionFinal = Vector3.left; // Caso Izquierda
                    else direccionFinal = Vector3.right; // Caso Derecha

                    BarreraHorizontal();
                    break;

                case 1:
                    Debug.Log("Disparo Vertical efectuado");

                    //Declaramos la direccion que tomara la bala
                    direccionFinal = Vector3.down;

                    BarreraVertical();
                    break;

                case 2:
                    Debug.Log("Disparo Diagonal efectuado");

                    //Declaramos la direccion que tomara la bala
                    if (playerPos < 0) direccionFinal = (Vector3.down + Vector3.left).normalized; // Caso Izquierda
                    else direccionFinal = (Vector3.down + Vector3.right).normalized; // Caso Derecha

                    BarreraDiagonal();
                    break;

                default:
                    Debug.LogWarning("No se ha elegido ningun tipo de disparo.");
                    break;
            }

            StartCoroutine(EsperaSpawner());
        }

    }

    private void BarreraHorizontal()
    {
        //Obtenemos el punto mas bajo del Boss para iniciar la primera bala
        float posicionBalaInicialY = (float)((GetComponent<MeshRenderer>().bounds.min.y) + 0.25);
        float posicionBalaInicialX;
        if (playerPos < 0) posicionBalaInicialX = transform.position.x - offsetBalaHorizontal_H; //Caso Izquierda
        else posicionBalaInicialX = transform.position.x + offsetBalaHorizontal_H; // Caso Derecha

        List<int> indicesSaltados = new List<int>();
        int primeraBala = (int)Random.Range(0 + huecoBarrera_H - 1, numeroDeBalas_H);
        for (int i = 0; i < huecoBarrera_H; i++)
        {
            indicesSaltados.Add(primeraBala - i);
        }

        for (int i = 0; i < numeroDeBalas_H; i++)
        {
            if (i != 0) posicionBalaInicialY += separacionEntreBalas_H;

            Vector3 posicionBala = new Vector3(posicionBalaInicialX, posicionBalaInicialY, transform.position.z);

            if (!indicesSaltados.Contains(i))
            {
                GameObject clon = Instantiate(bala, posicionBala, Quaternion.identity);
                clon.GetComponent<Bullet>().direccionDisparo = direccionFinal;
                clon.GetComponent<Bullet>().velocidad = velocidadBala_H;
                clon.GetComponent<Bullet>().tiempoVida = tiempoVidaBala_H;
            }
        }

        direccionFinal = Vector3.zero;

    }
    private void BarreraVertical()
    {
        //Obtenemos el punto mas bajo del Boss para iniciar la primera bala
        float posicionBalaInicialY = (float)((GetComponent<MeshRenderer>().bounds.max.y) + offsetBalaVertical_V);
        float posicionBalaInicialX;
        if (playerPos < 0) posicionBalaInicialX = transform.position.x - offsetBalaHorizontal_V; //Caso Izquierda
        else posicionBalaInicialX = transform.position.x + offsetBalaHorizontal_V; // Caso Derecha

        List<int> indicesSaltados = new List<int>();
        int primeraBala = (int)Random.Range(0 + huecoBarrera_V - 1, numeroDeBalas_V);
        for (int i = 0; i < huecoBarrera_V; i++)
        {
            indicesSaltados.Add(primeraBala - i);
        }

        for (int i = 0; i < numeroDeBalas_V; i++)
        {
            if (i != 0)
            {
                if (playerPos < 0) posicionBalaInicialX -= separacionEntreBalas_V;
                else posicionBalaInicialX += separacionEntreBalas_V;
            }
            

            Vector3 posicionBala = new Vector3(posicionBalaInicialX, posicionBalaInicialY, transform.position.z);

            if (!indicesSaltados.Contains(i))
            {
                GameObject clon = Instantiate(bala, posicionBala, Quaternion.identity);
                clon.GetComponent<Bullet>().direccionDisparo = direccionFinal;
                clon.GetComponent<Bullet>().velocidad = velocidadBala_V;
                clon.GetComponent<Bullet>().tiempoVida = tiempoVidaBala_V;
            }
        }

        direccionFinal = Vector3.zero;

    }
    private void BarreraDiagonal()
    {
        //Obtenemos el punto mas bajo del Boss para iniciar la primera bala
        float posicionBalaInicialY = (float)((GetComponent<MeshRenderer>().bounds.max.y) + offsetBalaVertical_D);
        float posicionBalaInicialX;
        if (playerPos < 0) posicionBalaInicialX = transform.position.x + offsetBalaHorizontal_D; //Caso Izquierda
        else posicionBalaInicialX = transform.position.x - offsetBalaHorizontal_D; // Caso Derecha

        List<int> indicesSaltados = new List<int>();
        int primeraBala = (int)Random.Range(0 + huecoBarrera_D - 1, numeroDeBalas_D);
        for (int i = 0; i < huecoBarrera_D; i++)
        {
            indicesSaltados.Add(primeraBala - i);
        }

        for (int i = 0; i < numeroDeBalas_D; i++)
        {
            if (i != 0)
            {
                if (playerPos < 0) posicionBalaInicialX -= separacionEntreBalas_D;
                else posicionBalaInicialX += separacionEntreBalas_D;
            }

            if (i != 0)
            {
                if (playerPos < 0) posicionBalaInicialX -= separacionEntreBalas_D;
                else posicionBalaInicialX += separacionEntreBalas_D;

                posicionBalaInicialY += separacionEntreBalas_D;
            }

            Vector3 posicionBala = new Vector3(posicionBalaInicialX, posicionBalaInicialY, transform.position.z);

            if (!indicesSaltados.Contains(i))
            {
                GameObject clon = Instantiate(bala, posicionBala, Quaternion.identity);
                clon.GetComponent<Bullet>().direccionDisparo = direccionFinal;
                clon.GetComponent<Bullet>().velocidad = velocidadBala_V;
                clon.GetComponent<Bullet>().tiempoVida = tiempoVidaBala_V;
            }
        }

        direccionFinal = Vector3.zero;
    }
    IEnumerator EsperaSpawner()
    {
        yield return new WaitForSeconds(tiempoEntreBarreras);
        puedeDisparar = true;
    }

}
