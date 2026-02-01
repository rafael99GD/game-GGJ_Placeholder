using UnityEngine;

public class PossessionManager : MonoBehaviour
{
    public static PossessionManager Instance;
    public PlayerMovementController player;
    public GhostController ghost;

    void Awake() => Instance = this;

    void Start() => SetControl(true);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            SetControl(!player.isControlEnabled);
    }

    public void SetControl(bool humanMode)
    {
        player.isControlEnabled = humanMode;
        ghost.isActive = !humanMode;

        if (ghost.isActive)
        {
            ghost.gameObject.SetActive(true);
            ghost.transform.position = player.transform.position;
        }
        else
        {
            ghost.gameObject.SetActive(false);
        }
    }
}