using UnityEngine;

public class PlayerPickup : MonoBehaviour
{

    [SerializeField] float interactDistance = 3f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        //Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        //if(Physics.Raycast(ray, out RaycastHit hit, interactDistance)){Debug.Log(hit.transform.name);}
        //Debug.DrawRay(cam.transform.position, cam.transform.forward * 3f, Color.red);
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }

    void TryPickup()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact(this);
            }
            
            WorldItems worldItem = hit.collider.GetComponent<WorldItems>();

            if (worldItem!= null)
            {
                if (InventorySystem.instance.AddItem(worldItem.itemData))
                {
                    Destroy(worldItem.gameObject);
                }
            }
        }
    }

    void TryInteract()
    {
        
    }
}
