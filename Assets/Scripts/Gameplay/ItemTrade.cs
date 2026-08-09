using UnityEngine;
using UnityEngine.Events;

public class ItemTrade : MonoBehaviour
{
    public static ItemTrade instance;
    [Header("Trade")]
    [SerializeField] KeyItemData itemToTake;
    [SerializeField] KeyItemData itemToGive;
    public bool removeOldItem = true;

    public UnityEvent onTradeComplete;

    void Awake()
    {
        if (instance != this && instance != null) return;
        instance = this;
    }

    public void ExecuteTrade(InventorySystem inventory)
    {
        if (removeOldItem)
        {
            inventory.RemoveKeyItem(itemToTake);
        }

        if (itemToGive != null)
        {
            inventory.AddKeyItem(itemToGive);
        }

        onTradeComplete?.Invoke();
        Debug.Log("Traded" + itemToGive);
    }
    
}
