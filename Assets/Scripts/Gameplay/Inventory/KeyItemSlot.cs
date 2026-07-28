using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyItemSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text itemname;

    private KeyItemData item;

    public void Setup(KeyItemData data)
    {
        item = data;

        icon.sprite = data.icon;
        itemname.text = data.itemName;
    }

    public KeyItemData GetItemData()
    {
        return item;
    }
}
