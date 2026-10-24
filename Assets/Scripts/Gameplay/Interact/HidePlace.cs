using ElmanGameDevTools.PlayerSystem;
using UnityEngine;

public class HidePlace : MonoBehaviour, IInteractable
{
    [SerializeField] Transform cameraPos;
    [SerializeField] PlayerController plrCont;

    bool plrInside = false;

    public void Interact(PlayerPickup player)
    {
        Debug.Log("interacted!");
        Debug.Log(player.transform);
        //if (plrCont != null) return;
        plrCont = player.transform.TryGetComponent<PlayerController>(out PlayerController pC) ? pC : null;
        ProcessHiding();
    }

    private void ProcessHiding()
    {
        if (plrCont == null) return;
        if (plrInside)
        {
            plrCont.controller.enabled = true;
            plrCont = null;
            plrInside = false;
        }
        else
        {
            plrCont.controller.enabled = false;
            plrInside = true;
        }
    }

    void Update()
    {
        if (plrCont == null) return;
        if(Input.GetKeyDown(KeyCode.E)) ProcessHiding();
    }
}
