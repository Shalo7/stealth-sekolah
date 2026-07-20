using UnityEngine;


public abstract class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;

    [TextArea(3,5)]
    public string itemDescription;

    public Sprite icon;
}
