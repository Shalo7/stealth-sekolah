using UnityEngine;
using UnityEngine.Events;

public class KeyItemRequired : MonoBehaviour, IInteractable
{
    [Header("Requirements")]
    public KeyItemData requiredItem;

    [Header("Events")]
    public UnityEvent onSuccess;
    public UnityEvent onFail;
    
    public void Interact(PlayerPickup player)
    {
        if (InventorySystem.instance.HasKeyItem(requiredItem)) onSuccess.Invoke();
        else onFail.Invoke();
    }
}
