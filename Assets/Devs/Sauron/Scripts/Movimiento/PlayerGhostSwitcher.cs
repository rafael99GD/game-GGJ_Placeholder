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
        SetState(false);
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
        humanoScript.enabled = !fantasmaActivo;
        fantasmaScript.enabled = fantasmaActivo;

        _rb.useGravity = !fantasmaActivo;

        if (fantasmaActivo)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.linearDamping = 0f;
        }
        else
        {
            _rb.linearDamping = 0.5f;
            transform.rotation = Quaternion.identity;
        }

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
            mat.SetOverrideTag("RenderType", "");
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.renderQueue = -1;
        }
    }
}
