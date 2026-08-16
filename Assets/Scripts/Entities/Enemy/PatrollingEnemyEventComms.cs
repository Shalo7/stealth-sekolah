using System;
using UnityEngine;

public class PatrollingEnemyEventComms : MonoBehaviour
{
    [SerializeField] EnemySightController enemySightController;
    [SerializeField] EnemyListenController enemyListenController;

    public Action<Transform> playerSightedComms;
    public Action<Vector3> enemyListenedComms;

    //Connect with all event callers so it can send it to the rightful receivers.
    void OnEnable()
    {
        enemySightController.playerSightedEvent += playerSightedMediator;
        enemyListenController.giveSoundPositionEvent += playerListenMediator;
    }

    void OnDisable()
    {
        enemySightController.playerSightedEvent -= playerSightedMediator;
        enemyListenController.giveSoundPositionEvent -= playerListenMediator;
    }

    private void playerListenMediator(Vector3 pos)
    {
        enemyListenedComms?.Invoke(pos);
    }

    //This sends the data from the EnemySightController to the PatrollingEnemyLogic when it sees something/nothing.
    private void playerSightedMediator(Transform target)
    {
        playerSightedComms?.Invoke(target);
    }
}
