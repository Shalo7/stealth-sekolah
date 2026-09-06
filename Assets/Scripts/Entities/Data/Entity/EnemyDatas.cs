using UnityEngine;

namespace EntityData.Enemy
{

    [System.Serializable] public struct EnemyOrderlyPatrolData
    {
        public Transform path;
        public bool stopNLook;
        public float lookTime;

        public EnemyOrderlyPatrolData(Transform p, bool sNL, float lT)
        {
            this.path = p;
            this.stopNLook = sNL;
            this.lookTime = lT;
        } 
    }
}
