using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyItemRequired : MonoBehaviour, IInteractable
{
    [Header("Requirements")]
    [SerializeField] List<KeyItemData> requiredItem = new();
    [SerializeField] bool tradeEvent = false;

    [Header("Events")]
    [SerializeField] UnityEvent onSuccess;
    [SerializeField] UnityEvent onFail;
    
    public void Interact(PlayerPickup player)
    {
        InventorySystem inventory = player.GetComponent<InventorySystem>();
        if (inventory.HasKeyItem(requiredItem))
        {
            if (tradeEvent)
            {
                ItemTrade.instance.ExecuteTrade(inventory);
                onSuccess.Invoke();
            }
            else
            {
                onSuccess.Invoke();
            }
            
        } 
        else onFail.Invoke();
    }
}
