using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyItemMenuUI : MonoBehaviour
{
    [Header("References")]

    public InventorySystem inventory;

    public Transform content;

    public KeyItemSlot slotPrefab;

    public Image largeIcon;

    public TMP_Text itemName;

    public TMP_Text description;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {

    }

    public void SelectItem(KeyItemData item)
    {

    }
}