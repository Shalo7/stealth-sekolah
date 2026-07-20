using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image icon;

    // public void SetItem(ItemsBase item)
    // {
    //     icon.enabled = true;
    //     icon.sprite = item.icon;
    // }

    public void Clear()
    {
        icon.enabled = false;
    }
}
