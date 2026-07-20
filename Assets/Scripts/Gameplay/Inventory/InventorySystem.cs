using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;
    public const int MaxThrowableItems = 4;
    public int selectedThrowableIndex = 0;

    public List<ThrowableItemData> throwableItems = new();
    public List<KeyItemData> keyItems = new(); 

    private void Awake()
    {
        if (instance != this && instance != null) return;
        instance = this;
    }

    public bool AddItem(ItemData item)
    {
        if (item is ThrowableItemData throwable)
        {
            if (throwableItems.Count >= MaxThrowableItems)
            {
                Debug.Log("Throwable inventory is full!");
                return false;
            }

            throwableItems.Add(throwable);
            return true;
        }

        if (item is KeyItemData key)
        {
            if (keyItems.Contains(key))
            {
                Debug.Log("Already have this key item.");
                return false;
            }

            keyItems.Add(key);
            return true;
        }

        return false;
    }
    
    public ThrowableItemData GetSelectedThrowable()
    {
        if (throwableItems.Count == 0) return null;

        selectedThrowableIndex = Mathf.Clamp(selectedThrowableIndex, 0, throwableItems.Count - 1);

        return throwableItems[selectedThrowableIndex];
    }

    public void RemoveSelectedThrowable()
    {
        if (throwableItems.Count == 0) return;

        throwableItems.RemoveAt(selectedThrowableIndex);

        if (selectedThrowableIndex >= throwableItems.Count) 
            selectedThrowableIndex = Mathf.Max(0, throwableItems.Count - 1);
    }
}
