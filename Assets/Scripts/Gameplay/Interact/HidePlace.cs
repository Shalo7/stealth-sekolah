using System.Collections;
using ElmanGameDevTools.PlayerSystem;
using Unity.VisualScripting;
using UnityEngine;

public class HidePlace : MonoBehaviour, IInteractable
{
    [SerializeField] Transform cameraPos;
    [SerializeField] PlayerController plrCont;
    [SerializeField] Transform plr;

    [SerializeField] bool plrInside = false;

    private void Start()
    {
        plr = PlayerMarker.instance.transform;
        if(plr.TryGetComponent<PlayerController>(out PlayerController n)) {Debug.Log("Yes!"); plrCont = n;}
        else {Debug.Log("No!");}
    }

    public void Interact(PlayerPickup player)
    {
        if (player.transform == plr) {Debug.Log("Player!");};
        ProcessHiding();
    }

    private void ProcessHiding()
    {
        Debug.Log("Start");
        if (plrCont == null) return;
        if (plrInside)
        {
            plrCont.controller.enabled = true;
            plrInside = false;
            if (CO_CheckInput != null)
            {
                StopCoroutine(CO_CheckInput);
                CO_CheckInput = null;
                delayCount = 0f;
            }
        }
        else
        {
            plrCont.controller.enabled = false;
            plrInside = true;
            if (CO_CheckInput == null)
            {
                CO_CheckInput = StartCoroutine(CheckInput());
            }
        }
    }

    float delayCount = 0f;
    float delayTime = 0.2f;
    Coroutine CO_CheckInput = null;
    IEnumerator CheckInput()
    {
        while (delayCount <= delayTime)
        {
            delayCount += Time.deltaTime;
            yield return null;
        }

        while (plrInside)
        {
            if (Input.GetKeyDown(KeyCode.E)) ProcessHiding();
            yield return null;
        }

        CO_CheckInput = null;
        delayCount = 0f;
    }
}
