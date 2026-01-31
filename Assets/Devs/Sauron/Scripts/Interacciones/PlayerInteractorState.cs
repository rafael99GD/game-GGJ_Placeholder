using UnityEngine;

public class PlayerInteractorState : MonoBehaviour
{
    public bool CanInteract { get; private set; } = true;

    public void EnableInteraction(bool value)
    {
        CanInteract = value;
    }
}
