using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Throwable Item")]
public class ThrowableItemData : ItemData
{
    public GameObject prefab;
    public float throwForce = 12f;
}
