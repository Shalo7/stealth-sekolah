using UnityEngine;

public class ThrowableSelector : MonoBehaviour
{
    InventorySystem inventory;
    HUDUi hud;

    void Start()
    {
        inventory = GetComponent<InventorySystem>();
        hud = FindFirstObjectByType<HUDUi>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Select(0);

        if (Input.GetKeyDown(KeyCode.Alpha2)) Select(1);

        if (Input.GetKeyDown(KeyCode.Alpha3)) Select(2);

        if (Input.GetKeyDown(KeyCode.Alpha4)) Select(3);

        float wheel = Input.GetAxis("Mouse ScrollWheel");

        if (wheel > 0) Next();

        if (wheel < 0) Previous();
    }

    void Select(int index)
    {
        if (index >= inventory.throwableItems.Count) return;

        inventory.selectedThrowableIndex = index;
        hud.Refresh();
    }

    void Next()
    {
        if (inventory.throwableItems.Count == 0) return;

        inventory.selectedThrowableIndex++;

        if (inventory.selectedThrowableIndex >= inventory.throwableItems.Count) inventory.selectedThrowableIndex = 0;

        hud.Refresh();
    }

    void Previous()
    {
        if (inventory.throwableItems.Count == 0) return;

        inventory.selectedThrowableIndex--;

        if (inventory.selectedThrowableIndex < 0) inventory.selectedThrowableIndex = inventory.throwableItems.Count - 1;

        hud.Refresh();
    }
}
