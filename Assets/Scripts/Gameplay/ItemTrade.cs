using UnityEngine;

public class ItemTrade : MonoBehaviour
{
    [Header("Trade")]

    [SerializeField] KeyItemData itemToTake;
    [SerializeField] KeyItemData itemToGive;
    public bool removeOldItem = true;
    
}
