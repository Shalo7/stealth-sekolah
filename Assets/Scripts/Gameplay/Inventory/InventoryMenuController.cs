using UnityEngine;

public class InventoryMenuController : MonoBehaviour
{
    public GameObject menu;

    bool isOpen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isOpen = !isOpen;

        menu.SetActive(isOpen);

        Time.timeScale = isOpen ? 0f : 1f;

        Cursor.visible = isOpen;

        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
