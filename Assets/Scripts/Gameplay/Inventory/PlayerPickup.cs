using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] float pickupDistance = 3f;

    private InventorySystem inventory;
    private Camera cam;

    void Start()
    {
        inventory = GetComponent<InventorySystem>();
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }

    void TryPickup()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            WorldItems worldItem = hit.collider.GetComponent<WorldItems>();

            if (worldItem!= null)
            {
                if (inventory.AddItem(worldItem.itemData))
                {
                    Destroy(worldItem.gameObject);
                }
            }
        }
    }
}
