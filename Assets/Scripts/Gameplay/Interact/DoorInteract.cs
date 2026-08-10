using System.Collections;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    [SerializeField] float openAngle = 90f;
    [SerializeField] float openSpeed = 0.5f;
    
    Transform doorPivot;

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

    IEnumerator RotateDoor(Quaternion targetRotation)
    {
        isMoving = true;

        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < openSpeed)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / openSpeed;

            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }
        
        transform.rotation = targetRotation;

        isMoving = false;
    }

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
