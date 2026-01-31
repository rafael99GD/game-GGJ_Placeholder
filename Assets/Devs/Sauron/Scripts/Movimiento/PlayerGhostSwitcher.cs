using UnityEngine;

public class PlayerGhostSwitcher : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerMovementController humanoScript;
    [SerializeField] private GhostMovement fantasmaScript;

    [Header("Visuals")]
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private float alphaFantasma = 0.4f;

    private bool esFantasma = false;
    private Rigidbody _rb;
    private Collider _col;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        SetState(false); // Empezamos como humano
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            esFantasma = !esFantasma;
            SetState(esFantasma);
        }
    }

    void SetState(bool fantasmaActivo)
    {
        // 1. Activar/Desactivar Scripts
        humanoScript.enabled = !fantasmaActivo;
        fantasmaScript.enabled = fantasmaActivo;

        // 2. Configurar Físicas
        _rb.useGravity = !fantasmaActivo;

        if (fantasmaActivo)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.linearDamping = 0f; // Evita que se frene solo en el aire
        }
        else
        {
            _rb.linearDamping = 0.5f; // Recupera fricción normal si quieres
            transform.rotation = Quaternion.identity; // Asegura que vuelva derecho
        }

        // 3. Visuales
        ActualizarVisuales(fantasmaActivo);
    }

    void ActualizarVisuales(bool esFantasma)
    {
        if (playerRenderer == null) return;

        Material mat = playerRenderer.material;
        Color c = mat.color;

        if (esFantasma)
        {
            c.a = alphaFantasma;
            mat.color = c;
            // Configuración Standard Shader para Transparencia
            mat.SetOverrideTag("RenderType", "Transparent");
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
        else
        {
            c.a = 1f;
            mat.color = c;
            // Volver a modo Opaque
            mat.SetOverrideTag("RenderType", "");
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.renderQueue = -1;
        }
    }
}