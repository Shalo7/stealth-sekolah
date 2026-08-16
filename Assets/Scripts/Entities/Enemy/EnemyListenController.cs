using System;
using UnityEngine;

public class EnemyListenController : MonoBehaviour
{
    public Action<Vector3> giveSoundPositionEvent;

    public void TriggerListen(Vector3 pos)
    {
        giveSoundPositionEvent?.Invoke(pos);
    }
}
