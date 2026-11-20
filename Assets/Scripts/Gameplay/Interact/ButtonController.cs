
using UnityEngine;

public class ButtonController : MonoBehaviour, IInteractable
{
    [Header("Interaction Target")]
    [SerializeField] private SlidingGate targetObject;

    public void Interact(PlayerPickup player)
    {
        if (targetObject != null)
        {
            targetObject.Interact(player);
        }
    }
}
