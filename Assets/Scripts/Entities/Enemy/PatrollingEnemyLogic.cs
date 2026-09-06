using System;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using EntityData.Enemy;
using System.Collections.Generic;

public class PatrollingEnemyLogic : MonoBehaviour
{
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] PlayerMarker plr;
    [SerializeField] LayerMask groundLayer, playerLayer;
    [SerializeField] float patrolWalkPointRange;
    [SerializeField] float searchWalkPointRange;
    [SerializeField] float patrolRestDuration;
    [SerializeField] float investigateRestDuration;
    [SerializeField] float searchRestDuration;
    [SerializeField] float chaseSpd = 7f;
    [SerializeField] float walkSpd = 3.5f;
    [SerializeField] List<EntityData.Enemy.EnemyOrderlyPatrolData> patrolRoute = new List<EnemyOrderlyPatrolData>();
    [SerializeField] int currentRoute = 0;
    [SerializeField] Animator animator;

    [SerializeField] ENEMYSTATES currentState;
    public ENEMYSTATES GetCurrentEnemyState () => currentState;
    [SerializeField] ENEMYSTATES defaultState = ENEMYSTATES.IDLE;
    
    [SerializeField] PatrollingEnemyEventComms patrollingEnemyEventComms;
    [SerializeField] PatrollingEnemyAlertController patrollingEnemyAlertController;
    Transform seenTarget;

    private Vector3 patrolWalkPoint;
    private bool patrolWalkPointSet;
    private Vector3 investigatePoint;
    private bool investigatePointSet;
    private bool searchPointSet;

    void OnEnable()
    {
        patrollingEnemyEventComms.playerSightedComms += playerSightedReceiver;
        patrollingEnemyEventComms.enemyListenedComms += enemyListenedReceiver;
    }


    void Start()
    {
        plr = PlayerMarker.instance;
        patrollingEnemyAlertController = transform.GetComponentInChildren<PatrollingEnemyAlertController>();
    }

    //Update is called once per frame. Checks if alert bar is more than 0. if so, stay alert and search around. If not then proceed to default state. checks states per frames and runs the respective code tied to the state.
    void Update()
    {
        if (patrollingEnemyAlertController == null) {Debug.LogError("No alert controller!"); return;}
        if (patrollingEnemyAlertController.GetHighReadinessStatus())
        {
            if (currentState == ENEMYSTATES.PATROL || currentState == ENEMYSTATES.IDLE || currentState == ENEMYSTATES.STRUCTUREDPATROL) {currentState = ENEMYSTATES.SEARCH;}    
        }
        else
        {
            if (currentState != ENEMYSTATES.OBSERVING && currentState != ENEMYSTATES.OBSERVINGCOOLDOWN) currentState = defaultState;
        }

        if (currentState == ENEMYSTATES.PATROL) {Patroling(); return;}
        if (currentState == ENEMYSTATES.STRUCTUREDPATROL) {StructurelyPatrol(); return;}
        if (currentState == ENEMYSTATES.INVESTIGATEWALK) {Investigate(); return;}
        if (currentState == ENEMYSTATES.OBSERVING) {ObserveTarget(); return;}
        if (currentState == ENEMYSTATES.OBSERVINGCOOLDOWN) {ObserveTargetCooldown(); return;}
        if (currentState == ENEMYSTATES.SEARCH) {Search(); return;}
        if (currentState == ENEMYSTATES.CHASE) {Chase(); return;}
    }


    private void enemyListenedReceiver(Vector3 vector)
    {
        if (patrollingEnemyAlertController == null) {Debug.LogError("No alert controller");return;}
        investigatePoint = vector;
        investigatePointSet = true;
        currentState = ENEMYSTATES.INVESTIGATEWALK;
        patrollingEnemyAlertController.AddAlertBar(10f);
    }

    //receive info that enemy spots a player
    private void playerSightedReceiver(Transform target)
    {
        if (target != null)
        {
            if (currentState == ENEMYSTATES.CHASE) return;
            currentState = ENEMYSTATES.OBSERVING;
            seenTarget = target;
            //print(seenTarget);
        }
        else
        {
            if (seenTarget == null) return;
            if (seenTarget != null) 
            {
                if (currentState == ENEMYSTATES.CHASE)
                {
                    investigatePointSet = true;
                    investigatePoint = seenTarget.transform.position;
                }
            }
            seenTarget = null;
        }
    }

    //checks if enemy has patrol point, if not generate one and walk towards it.
    private float patrolRestCounter = 0f;
    private void Patroling()
    {
        navAgent.speed = walkSpd;
        investigatePointSet = false;
        searchPointSet = false;
        if (!patrolWalkPointSet)
        {
            SearchPatrolWalkPoint();
        }

        if (patrolWalkPointSet)
        {
            navAgent.SetDestination(patrolWalkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - patrolWalkPoint;
        //animator.SetFloat("Velocity", 0.2f);

        if (distanceToWalkPoint.magnitude <= Mathf.Abs(0.1f))
        {
            if (patrolRestCounter <= patrolRestDuration) {patrolRestCounter += Time.deltaTime; return;}
            patrolWalkPointSet = false;
            patrolRestCounter = 0f;
        }
    }

    //generates a patrol walk point
    private void SearchPatrolWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-patrolWalkPointRange, patrolWalkPointRange);
        float randomX = UnityEngine.Random.Range(-patrolWalkPointRange, patrolWalkPointRange);
        patrolWalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        print(patrolWalkPoint);
        patrolWalkPointSet = true;

        /*if (Physics.Raycast(patrolWalkPoint, -transform.up, 2f, groundLayer))
        {
            patrolWalkPointSet = true;
        }*/
    }

    //check if they enemy has a target to investigate, if not then return to default state.
    private float investigateRestCounter = 0f;
    private void Investigate()
    {
        if (patrollingEnemyAlertController.GetHighReadinessStatus()) {navAgent.speed = chaseSpd;}
        else {navAgent.speed = walkSpd;}
        searchPointSet = false;
        patrolWalkPointSet = false;
        if (!investigatePointSet)
        {
            currentState = defaultState;
        }

        if (investigatePointSet)
        {
            navAgent.SetDestination(investigatePoint);
        }

        Vector3 distanceToWalkPoint = transform.position - investigatePoint;
        //animator.SetFloat("Velocity", 0.2f);

        if (distanceToWalkPoint.magnitude <= Mathf.Abs(0.1f))
        {
            if (investigateRestCounter <= investigateRestDuration) {investigateRestCounter += Time.deltaTime; return;}
            investigatePointSet = false;
            investigateRestCounter = 0f;
            currentState = ENEMYSTATES.SEARCH;
        }

    }

    //check if enemy has target position to search for, if not generate then walk over it.
    private float searchRestCounter;
    private void Search()
    {
        navAgent.speed = walkSpd;
        investigatePointSet = false;
        patrolWalkPointSet = false;
        Vector3 destination = Vector3.zero;
        if (!searchPointSet)
        {
            float randomZ = UnityEngine.Random.Range(-patrolWalkPointRange, patrolWalkPointRange);
            float randomX = UnityEngine.Random.Range(-patrolWalkPointRange, patrolWalkPointRange);
            destination = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            navAgent.SetDestination(destination);
            searchPointSet = true;
        }

        Vector3 distanceToWalkPoint = transform.position - navAgent.destination;
        //animator.SetFloat("Velocity", 0.2f);

        if (distanceToWalkPoint.magnitude <= Mathf.Abs(0.1f))
        {
            if (searchRestCounter <= searchRestDuration) {searchRestCounter += Time.deltaTime; return;}
            searchPointSet = false;
            searchRestCounter = 0f;
        }
    }

    //Run this code when player is spotted.
    float alertDelayCounter = 0f;
    float alertDuration = 0.5f;
    private void ObserveTarget()
    {
        //print("observing!");
        if (seenTarget == null) 
        {
            currentState = ENEMYSTATES.OBSERVINGCOOLDOWN;
            return;
        }

        navAgent.SetDestination(transform.position);
        if (patrollingEnemyAlertController.GetHighReadinessStatus()) {patrollingEnemyAlertController.AddAlertBar(1000); currentState = ENEMYSTATES.CHASE; return;}
        if (patrollingEnemyAlertController.GetCurrentAlertBar() >= patrollingEnemyAlertController.GetCurrentMaxAlertBar())
        {currentState = ENEMYSTATES.CHASE; return;}

        var v3_Dir = seenTarget.position - transform.position;
        var v3_TargetRotation = Quaternion.LookRotation(v3_Dir);
        var flt_Lookspd = 12f;
        transform.rotation = Quaternion.Slerp(transform.rotation, v3_TargetRotation, Time.deltaTime * flt_Lookspd);
        
        float dist = (seenTarget.transform.position - transform.position).magnitude;
        if (dist <= 6f) { patrollingEnemyAlertController.AddAlertBar(1000); return;}
        if (alertDelayCounter <= alertDuration)
        {
            alertDelayCounter += Time.deltaTime;
            //print("counting!");
        }
        else
        {
            patrollingEnemyAlertController.AddAlertBar(7.5f);
            //print("added value!");
            alertDelayCounter = 0f;
        }
    }

    //if the enemy lost sight of the player while observing run this code.
    private void ObserveTargetCooldown()
    {
        if (patrollingEnemyAlertController.GetCurrentAlertBar() <= 0) {currentState = defaultState;}
    }
    
    //chase the player
    private void Chase()
    {
        navAgent.speed = chaseSpd;
        if (seenTarget != null)
        {
            navAgent.SetDestination(seenTarget.transform.position);
            var v3_Dir = seenTarget.position - transform.position;
            var v3_TargetRotation = Quaternion.LookRotation(v3_Dir);
            var flt_Lookspd = 24f;
            transform.rotation = Quaternion.Slerp(transform.rotation, v3_TargetRotation, Time.deltaTime * flt_Lookspd);
        }

        Vector3 distanceToPoint = transform.position - navAgent.destination;
        if (distanceToPoint.magnitude <= Mathf.Abs(0.1f))
        {
            if (seenTarget == null)
            { currentState = defaultState; }
        }
    }

    //patrols its set path.
    float StopNLookCount = 0f;
    private void StructurelyPatrol()
    {
        if (patrolRoute.Count <= 0) {Debug.LogWarning("No routes!"); return;}
        EnemyOrderlyPatrolData currentPath = patrolRoute[currentRoute];
        Vector3 distanceToTarget = currentPath.path.position - transform.position;

        if (distanceToTarget.magnitude <= Mathf.Abs(0.1f))
        {
            if (!currentPath.stopNLook)
            {
                if (currentRoute < (patrolRoute.Count - 1)) currentRoute++;
                else {currentRoute = 0;}
            }
            else
            {
                if (StopNLookCount <= currentPath.lookTime) {StopNLookCount += Time.deltaTime; return;}

                StopNLookCount = 0;
                if (currentRoute < (patrolRoute.Count - 1)) currentRoute++;
                else {currentRoute = 0;}
            }
        }
        else
        {
            navAgent.destination = currentPath.path.position;
        }
    }
}
