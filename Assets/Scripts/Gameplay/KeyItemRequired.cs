using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyItemRequired : MonoBehaviour, IInteractable
{
    [Header("Requirements")]
    public List<KeyItemData> requiredItem = new();

    [Header("Events")]
    public UnityEvent onSuccess;
    public UnityEvent onFail;
    
    public void Interact(PlayerPickup player)
    {
        InventorySystem inventory = player.GetComponent<InventorySystem>();
        if (inventory.HasKeyItem(requiredItem))
        {
            ItemTrade.instance.ExecuteTrade(inventory);
            onSuccess.Invoke();
        } 
        else onFail.Invoke();
    }
}
