using UnityEngine;

public abstract class BaseSoundTriggerMonitor : MonoBehaviour
{
    [SerializeField] protected private float totalSoundEnergy;
    protected private abstract void MonitorSoundTriggers();
}
