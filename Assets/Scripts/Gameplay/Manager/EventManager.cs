using UnityEngine;

public class EventManager : MonoBehaviour
{
    public void UnlockDoor()
    {
        DoorInteract door = GetComponent<DoorInteract>();
        door.enabled = true;
    }

    public void LockedDoor()
    {
        Debug.Log("Need Key");
    }
}
