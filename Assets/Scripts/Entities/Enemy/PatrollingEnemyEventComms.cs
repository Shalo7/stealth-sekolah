using System;
using UnityEngine;

public class PatrollingEnemyEventComms : MonoBehaviour
{
    [SerializeField] EnemySightController enemySightController;

    public Action<Transform> playerSightedComms;

    //Connect with all event callers so it can send it to the rightful receivers.
    void OnEnable()
    {
        enemySightController.playerSighted += playerSightedReceiver;
    }

    void OnDisable()
    {
        enemySightController.playerSighted -= playerSightedReceiver;
    }

    //This sends the data from the EnemySightController to the PatrollingEnemyLogic when it sees something/nothing.
    private void playerSightedReceiver(Transform target)
    {
        playerSightedComms?.Invoke(target);
    }
}
