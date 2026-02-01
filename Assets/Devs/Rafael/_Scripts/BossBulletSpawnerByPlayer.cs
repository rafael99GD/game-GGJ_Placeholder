using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FaseBoss
{
    public string nombreFase = "Nombre de la Fase";
    public float tiempoDeEstaFase = 100f;
    public float bajaPorSegundo = 1f;
    public float tiempoEntreBarreras = 2f;

    [Tooltip("Cuanto tiempo se recupera el Boss cuando una bala toca al jugador en esta fase")]
    public float incrementoTiempoPorGolpe = 5f;

    [Header("Tipos de Disparo Activos")]
    public bool horizontal = true;
    public bool vertical = true;
    public bool diagonal = true;

    [Header("Configuracion Horizontal")]
    public float offset_H = 5f;
    public float numeroDeBalas_H = 10f;
    public float separacion_H = 1.2f;
    public float hueco_H = 2f;
    public int velocidad_H = 14;
    public int tiempoVida_H = 8;

    [Header("Configuracion Vertical")]
    public float offsetH_V = 0f;
    public float offsetV_V = 5f;
    public float numeroDeBalas_V = 10f;
    public float separacion_V = 1.2f;
    public float hueco_V = 2f;
    public int velocidad_V = 14;
    public int tiempoVida_V = 8;

    [Header("Configuracion Diagonal")]
    public float offsetH_D = 5f;
    public float offsetV_D = 5f;
    public float numeroDeBalas_D = 10f;
    public float separacion_D = 1.2f;
    public float hueco_D = 2f;
    public int velocidad_D = 14;
    public int tiempoVida_D = 8;
}

public class BossBulletSpawnerByPlayer : MonoBehaviour
{
    [Header("Configuracion de Fases")]
    [SerializeField] private FaseBoss[] fases;
    private int indiceFaseActual = 0;

    [Header("Parametros de Vidas")]
    [SerializeField] private Image[] corazonesUI;
    private int vidasActuales;

    [Header("UI y Objetos")]
    [SerializeField] private Slider barraProgreso;
    [SerializeField] private GameObject bala;
    [SerializeField] private bool activado;

    [Header("Visual Parpadeo (Nuevo)")]
    [SerializeField] private SpriteRenderer bossSprite; // Arrastra aquí el objeto visual del Boss
    [SerializeField] private Color colorParpadeo = Color.red;
    [SerializeField] private float velocidadParpadeo = 0.2f;

    private float tiempoActual;
    private bool esperandoGolpeFantasma = false;
    private bool puedeDisparar;
    private float playerPos;
    private GameObject player;

    private Coroutine corrutinaParpadeo;
    private Color colorOriginal;

    // Propiedad para obtener la configuración de la fase que se está jugando
    public FaseBoss FaseActual => fases[Mathf.Clamp(indiceFaseActual, 0, fases.Length - 1)];

    private void Start()
    {
        puedeDisparar = true;
        player = GameObject.FindWithTag("Player");
        vidasActuales = corazonesUI.Length;

        // Guardamos el color inicial del sprite
        if (bossSprite != null)
            colorOriginal = bossSprite.color;
        else
            Debug.LogWarning("BossBulletSpawner: No has asignado el SpriteRenderer del Boss.");

        if (fases.Length > 0) ConfigurarFase(0);
    }

    private void ConfigurarFase(int indice)
    {
        // Resetear visuales por si venimos de un estado de parpadeo
        DetenerParpadeo();

        indiceFaseActual = indice;
        tiempoActual = FaseActual.tiempoDeEstaFase;

        if (barraProgreso != null)
        {
            barraProgreso.maxValue = FaseActual.tiempoDeEstaFase;
            barraProgreso.value = tiempoActual;
        }
    }

    private void Update()
    {
        if (!activado || esperandoGolpeFantasma) return;

        ManejarCronometro();

        if (puedeDisparar) SeleccionarDisparoAleatorio();
    }

    private void ManejarCronometro()
    {
        if (tiempoActual > 0)
        {
            tiempoActual -= FaseActual.bajaPorSegundo * Time.deltaTime;
            if (barraProgreso != null) barraProgreso.value = tiempoActual;
        }
        else if (!esperandoGolpeFantasma)
        {
            // Entramos en estado de vulnerabilidad
            esperandoGolpeFantasma = true;

            // Iniciamos el parpadeo en rojo
            if (bossSprite != null)
                corrutinaParpadeo = StartCoroutine(ProcesoParpadeo());
        }
    }

    private IEnumerator ProcesoParpadeo()
    {
        while (esperandoGolpeFantasma)
        {
            bossSprite.color = colorParpadeo;
            yield return new WaitForSeconds(velocidadParpadeo);
            bossSprite.color = colorOriginal;
            yield return new WaitForSeconds(velocidadParpadeo);
        }
    }

    private void DetenerParpadeo()
    {
        if (corrutinaParpadeo != null) StopCoroutine(corrutinaParpadeo);
        if (bossSprite != null) bossSprite.color = colorOriginal;
    }

    public void AplicarPenalizacionJugador()
    {
        if (esperandoGolpeFantasma) return;

        float cantidad = FaseActual.incrementoTiempoPorGolpe;
        tiempoActual = Mathf.Clamp(tiempoActual + cantidad, 0, FaseActual.tiempoDeEstaFase);

        if (barraProgreso != null) barraProgreso.value = tiempoActual;
    }

    public void RecibirDañoFantasma()
    {
        if (!esperandoGolpeFantasma) return;

        // Al recibir daño, paramos el efecto visual inmediatamente
        DetenerParpadeo();

        vidasActuales--;
        ActualizarCorazonesUI();

        if (vidasActuales <= 0)
        {
            Debug.Log("MUERTO");
            activado = false;
        }
        else
        {
            esperandoGolpeFantasma = false;
            ConfigurarFase(indiceFaseActual + 1);

            if (PossessionManager.Instance != null)
                PossessionManager.Instance.SetControl(true);
        }
    }

    private void ActualizarCorazonesUI()
    {
        for (int i = 0; i < corazonesUI.Length; i++)
        {
            if (corazonesUI[i] != null)
                corazonesUI[i].enabled = (i < vidasActuales);
        }
    }

    private void SeleccionarDisparoAleatorio()
    {
        if (player == null) return;

        playerPos = player.transform.position.x - transform.position.x;
        puedeDisparar = false;

        List<int> ops = new List<int>();
        if (FaseActual.horizontal) ops.Add(0);
        if (FaseActual.vertical) ops.Add(1);
        if (FaseActual.diagonal) ops.Add(2);

        if (ops.Count > 0)
        {
            int sel = ops[Random.Range(0, ops.Count)];
            if (sel == 0) BarreraHorizontal(playerPos < 0 ? Vector3.left : Vector3.right);
            else if (sel == 1) BarreraVertical(Vector3.down);
            else BarreraDiagonal((playerPos < 0 ? (Vector3.down + Vector3.left) : (Vector3.down + Vector3.right)).normalized);

            StartCoroutine(EsperaEntreDisparos());
        }
    }

    // --- MÉTODOS DE BARRERAS (Se mantienen igual) ---

    private void BarreraHorizontal(Vector3 dir)
    {
        float posInicY = (float)(GetComponent<MeshRenderer>().bounds.min.y + 0.25f);
        float posInicX = (playerPos < 0) ? transform.position.x - FaseActual.offset_H : transform.position.x + FaseActual.offset_H;
        int huecoCentral = (int)Random.Range(FaseActual.hueco_H - 1, FaseActual.numeroDeBalas_H);

        for (int i = 0; i < FaseActual.numeroDeBalas_H; i++)
        {
            if (i != 0) posInicY += FaseActual.separacion_H;
            if (i < huecoCentral - (FaseActual.hueco_H / 2) || i > huecoCentral + (FaseActual.hueco_H / 2))
                CrearBala(new Vector3(posInicX, posInicY, transform.position.z), dir, FaseActual.velocidad_H, FaseActual.tiempoVida_H);
        }
    }

    private void BarreraVertical(Vector3 dir)
    {
        float posInicY = (float)(GetComponent<MeshRenderer>().bounds.max.y + FaseActual.offsetV_V);
        float posInicX = (playerPos < 0) ? transform.position.x - FaseActual.offsetH_V : transform.position.x + FaseActual.offsetH_V;
        int huecoCentral = (int)Random.Range(FaseActual.hueco_V - 1, FaseActual.numeroDeBalas_V);

        for (int i = 0; i < FaseActual.numeroDeBalas_V; i++)
        {
            if (i != 0) posInicX += (playerPos < 0) ? -FaseActual.separacion_V : FaseActual.separacion_V;
            if (i < huecoCentral - (FaseActual.hueco_V / 2) || i > huecoCentral + (FaseActual.hueco_V / 2))
                CrearBala(new Vector3(posInicX, posInicY, transform.position.z), dir, FaseActual.velocidad_V, FaseActual.tiempoVida_V);
        }
    }

    private void BarreraDiagonal(Vector3 dir)
    {
        float posInicY = (float)(GetComponent<MeshRenderer>().bounds.max.y + FaseActual.offsetV_D);
        float posInicX = (playerPos < 0) ? transform.position.x + FaseActual.offsetH_D : transform.position.x - FaseActual.offsetH_D;
        int huecoCentral = (int)Random.Range(FaseActual.hueco_D - 1, FaseActual.numeroDeBalas_D);

        for (int i = 0; i < FaseActual.numeroDeBalas_D; i++)
        {
            if (i != 0) { posInicX += (playerPos < 0) ? -FaseActual.separacion_D : FaseActual.separacion_D; posInicY += FaseActual.separacion_D; }
            if (i < huecoCentral - (FaseActual.hueco_D / 2) || i > huecoCentral + (FaseActual.hueco_D / 2))
                CrearBala(new Vector3(posInicX, posInicY, transform.position.z), dir, FaseActual.velocidad_D, FaseActual.tiempoVida_D);
        }
    }

    private void CrearBala(Vector3 pos, Vector3 dir, int vel, int vida)
    {
        GameObject clon = Instantiate(bala, pos, Quaternion.identity);
        Bullet script = clon.GetComponent<Bullet>();
        if (script != null)
        {
            script.bossParent = this;
            script.direccionDisparo = dir;
            script.velocidad = vel;
            script.tiempoVida = vida;
        }
    }

    IEnumerator EsperaEntreDisparos()
    {
        yield return new WaitForSeconds(FaseActual.tiempoEntreBarreras);
        puedeDisparar = true;
    }
}