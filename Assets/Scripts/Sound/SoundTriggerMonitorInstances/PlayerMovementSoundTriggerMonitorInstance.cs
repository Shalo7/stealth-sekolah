using System;
using ElmanGameDevTools.PlayerSystem;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementSoundTriggerMonitorInstance : BaseSoundTriggerMonitor
{
    [SerializeField] PlayerController plrCont;
    [SerializeField] EntitySoundController entitySoundController;

    [SerializeField] float walkSoundEnergy;
    [SerializeField] float runSoundEnergy;
    [SerializeField] float crouchSoundEnergy;

    private protected override void MonitorSoundTriggers()
    {
        if (entitySoundController == null) return;

        switch (plrCont.GetMovementState())
        {
            default:
                totalSoundEnergy = 0f;
                break;
            case PlayerController.MovementState.Crouching:
                totalSoundEnergy = crouchSoundEnergy;
                break;
            case PlayerController.MovementState.Walking:
                totalSoundEnergy = walkSoundEnergy;
                break;
            case PlayerController.MovementState.Running:
                totalSoundEnergy = runSoundEnergy;
                break;
        }

        entitySoundController.AddSoundEnergy(totalSoundEnergy);
    }

    void Update()
    {
        if (plrCont == null) return;
        MonitorSoundTriggers();
    }
}
