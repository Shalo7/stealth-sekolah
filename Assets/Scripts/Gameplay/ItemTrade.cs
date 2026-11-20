using UnityEngine;
using UnityEngine.Events;

public class ItemTrade : MonoBehaviour
{
    [Header("Trade")]
    [SerializeField] KeyItemData itemToTake;
    [SerializeField] KeyItemData itemToGive;
    public bool removeOldItem = true;

    public UnityEvent onTradeComplete;

    public void ExecuteTrade(InventorySystem inventory)
    {
        if (removeOldItem)
        {
            inventory.RemoveKeyItem(itemToTake);
            Debug.Log("Gives" + itemToTake);
        }

        if (itemToGive != null)
        {
            inventory.AddKeyItem(itemToGive);
            Debug.Log("Traded" + itemToGive);
        }

        onTradeComplete?.Invoke();
    }
    
}
