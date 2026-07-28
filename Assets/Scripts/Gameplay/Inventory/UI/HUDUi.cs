using UnityEngine;

public class HUDUi : MonoBehaviour
{
    public HUDUIslot[] slots;

    //[SerializeField] InventorySystem inventory;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < InventorySystem.instance.throwableItems.Count)
                slots[i].SetItem(InventorySystem.instance.throwableItems[i]);
            else
                slots[i].Clear();

            slots[i].SetSelected(i == InventorySystem.instance.selectedThrowableIndex);
        }
    }
}
