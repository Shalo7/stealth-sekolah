using System.Collections;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    [SerializeField] float openAngle = 90f;
    [SerializeField] float openSpeed = 0.5f;
    
    [SerializeField] Transform doorPivot;

    bool isOpen = false;
    bool isMoving = false;
    
    Quaternion closedRotation;
    Quaternion openRotation;

    void Awake()
    {
        doorPivot = transform.parent;
        closedRotation = doorPivot.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    //rotates the door hinge so the door opens on its pivot
    IEnumerator RotateDoor(Quaternion targetRotation)
    {
        isMoving = true;

        Quaternion startRotation = doorPivot.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < openSpeed)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / openSpeed;

            doorPivot.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }
        
        doorPivot.rotation = targetRotation;

        isMoving = false;
    }

    //opens the door. Checks if its in the process of opening. If not, open/close it.
    public void OpenDoor()
    {
        Debug.Log("Door Opened");
        if (isMoving) return;

        if (!isOpen)
        {
            StartCoroutine(RotateDoor(openRotation));
            isOpen = true;
        }
        else
        {
            StartCoroutine(RotateDoor(closedRotation));
            isOpen = false;
        }
    }
}
