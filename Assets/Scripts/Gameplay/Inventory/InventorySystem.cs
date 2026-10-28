using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;
    public const int MaxThrowableItems = 4;
    public int selectedThrowableIndex = 0;
    [SerializeField] GameObject keyItemMenu;

    public List<ThrowableItemData> throwableItems = new();
    public List<KeyItemData> keyItems = new(); 

    bool isOpen;

    private void Awake()
    {
        if (instance != this && instance != null) return;
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("You Have" + string.Join(", ", keyItems));
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isOpen = !isOpen;
        keyItemMenu.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f;
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
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
            return AddKeyItem(key);
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

    public bool HasKeyItem(List<KeyItemData> keyItem)
    {
        foreach (var item in keyItem)
        {
            if (!keyItems.Contains(item)) return false;
        }
        return true;
    }

    public bool AddKeyItem(KeyItemData item)
    {
        if (keyItems.Contains(item)) return false;
        keyItems.Add(item);
        Debug.Log(item + "added");
        return true;
    }

    public bool RemoveKeyItem(KeyItemData item)
    {
        return keyItems.Remove(item);
    }
}
