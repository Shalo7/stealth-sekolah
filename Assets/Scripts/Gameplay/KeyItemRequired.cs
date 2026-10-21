using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyItemRequired : MonoBehaviour, IInteractable
{
    [Header("Requirements")]
    [SerializeField] List<KeyItemData> requiredItem = new();
    [SerializeField] bool tradeEvent = false;
    [SerializeField] ItemTrade itemTrade;

    [Header("Events")]
    [SerializeField] UnityEvent onSuccess;
    [SerializeField] UnityEvent onFail;
    
    public void Interact(PlayerPickup player)
    {
        Debug.Log("Interacted");
        InventorySystem inventory = player.GetComponent<InventorySystem>();
        if (inventory.HasKeyItem(requiredItem))
        {
            Debug.Log("success ;)");
            if (tradeEvent && itemTrade != null)
            {
                itemTrade.ExecuteTrade(inventory);
                onSuccess.Invoke();
            }
            else
            {
                onSuccess.Invoke();
            }
            
        } 
        else {onFail.Invoke(); Debug.Log("fail ;(");}
    }
}
