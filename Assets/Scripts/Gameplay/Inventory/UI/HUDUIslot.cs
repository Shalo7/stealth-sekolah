using UnityEngine;
using UnityEngine.UI;

public class HUDUIslot : MonoBehaviour
{
    public Image icon;
    [SerializeField] Image background;

    [SerializeField] Color selectedColor = Color.yellow;
    [SerializeField] Color normalColor = Color.white;

    public void SetItem(ThrowableItemData item)
    {
        icon.enabled = true;
        icon.sprite = item.icon;
    }

    public void Clear()
    {
        icon.enabled = false;
        icon.sprite = null;
    }

    public void SetSelected(bool selected)
    {
        background.color = selected ? selectedColor : normalColor;
    }
}
