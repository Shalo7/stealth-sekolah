using UnityEngine;

namespace EntityData.Enemy
{

    [System.Serializable] public struct EnemyOrderlyPatrolData
    {
        public Transform path;
        public bool stopNLook;
        public float lookTime;
        public bool lookFirst;
        public Transform lookDirection;

        public EnemyOrderlyPatrolData(Transform p, bool sNL, float lT, bool lF, Transform lD)
        {
            this.path = p;
            this.stopNLook = sNL;
            this.lookTime = lT;
            this.lookFirst = lF;
            this.lookDirection = lD;
        } 
    }
}
