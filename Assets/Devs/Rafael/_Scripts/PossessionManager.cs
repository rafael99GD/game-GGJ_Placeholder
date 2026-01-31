using UnityEngine;

public class PossessionManager : MonoBehaviour
{
    public static PossessionManager Instance; // Permite que otros scripts lo llamen fácilmente

    public PlayerMovementController player;
    public GhostController ghost;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Estado inicial: Jugador activo
        SetControl(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Cambia al estado opuesto al actual
            SetControl(!player.isControlEnabled);
        }
    }

    // Función centralizada para cambiar entre estados
    public void SetControl(bool stateToHuman)
    {
        player.isControlEnabled = stateToHuman;
        ghost.isActive = !stateToHuman;

        if (ghost.isActive)
        {
            ghost.gameObject.SetActive(true);
            ghost.transform.position = player.transform.position;
        }
        else
        {
            // El fantasma desaparece y el control vuelve al cuerpo físico
            ghost.gameObject.SetActive(false);
        }
    }
}