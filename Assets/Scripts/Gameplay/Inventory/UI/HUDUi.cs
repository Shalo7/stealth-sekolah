using UnityEngine;

public class HUDUi : MonoBehaviour
{
    public HUDUIslot[] slots;

    InventorySystem inventory;

    void Start()
    {
        inventory = FindFirstObjectByType<InventorySystem>();
        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.throwableItems.Count)
                slots[i].SetItem(inventory.throwableItems[i]);
            else
                slots[i].Clear();

            slots[i].SetSelected(i == inventory.selectedThrowableIndex);
        }
    }
}
