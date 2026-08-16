using System;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] Animator animator;

    [SerializeField] ENEMYSTATES currentState;
    public ENEMYSTATES GetCurrentEnemyState () => currentState;
    [SerializeField] ENEMYSTATES defaultState = ENEMYSTATES.IDLE;
    
    [SerializeField] PatrollingEnemyEventComms patrollingEnemyEventComms;
    [SerializeField] PatrollingEnemyAlertController patrollingEnemyAlertController;
    Transform seenTarget;

    private Vector3 patrolWalkPoint;
    private bool patrolWalkPointSet;
    private Vector3 investigateWalkPoint;
    private bool investigateWalkPointSet;
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
        if (patrollingEnemyAlertController.GetCurrentAlertBar() > 0)
        {
            if (currentState == ENEMYSTATES.PATROL || currentState == ENEMYSTATES.IDLE) {currentState = ENEMYSTATES.SEARCH;}    
        }
        else
        {
            if (currentState != ENEMYSTATES.OBSERVING) currentState = defaultState;
        }

        if (currentState == ENEMYSTATES.PATROL) {Patroling(); return;}
        if (currentState == ENEMYSTATES.INVESTIGATEWALK) {InvestigateWalk(); return;}
        if (currentState == ENEMYSTATES.OBSERVING) {ObserveTarget(); return;}
        if (currentState == ENEMYSTATES.SEARCH) {Search(); return;}
        if (currentState == ENEMYSTATES.CHASE) {Chase(); return;}
    }


    private void enemyListenedReceiver(Vector3 vector)
    {
        if (patrollingEnemyAlertController == null) {Debug.LogError("No alert controller");return;}
        investigateWalkPoint = vector;
        investigateWalkPointSet = true;
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
            print(seenTarget);
        }
        else
        {
            if (seenTarget == null) return;
            if (seenTarget != null) {navAgent.SetDestination(seenTarget.transform.position); currentState = defaultState;}
            seenTarget = null;
        }
    }

    private float patrolRestCounter = 0f;
    //checks if enemy has patrol point, if not generate one and walk towards it.
    private void Patroling()
    {
        navAgent.speed = walkSpd;
        investigateWalkPointSet = false;
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

    private float investigateRestCounter = 0f;
    //check if they enemy has a target to investigate, if not then return to default state.
    private void InvestigateWalk()
    {
        navAgent.speed = walkSpd;
        searchPointSet = false;
        patrolWalkPointSet = false;
        if (!investigateWalkPointSet)
        {
            currentState = defaultState;
        }

        if (investigateWalkPointSet)
        {
            navAgent.SetDestination(investigateWalkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - investigateWalkPoint;
        //animator.SetFloat("Velocity", 0.2f);

        if (distanceToWalkPoint.magnitude <= Mathf.Abs(0.1f))
        {
            if (investigateRestCounter <= investigateRestDuration) {investigateRestCounter += Time.deltaTime; return;}
            investigateWalkPointSet = false;
            investigateRestCounter = 0f;
            currentState = ENEMYSTATES.SEARCH;
        }
    }

    private float searchRestCounter;
    //check if enemy has target position to search for, if not generate then walk over it.
    private void Search()
    {
        navAgent.speed = walkSpd;
        investigateWalkPointSet = false;
        patrolWalkPointSet = false;
        Vector3 destination = Vector3.zero;
        if (!searchPointSet)
        {
            float randomZ = UnityEngine.Random.Range(-patrolWalkPointRange, patrolWalkPointRange);
            float randomX = UnityEngine.Random.Range(-patrolWalkPointRange, patrolWalkPointRange);
            destination = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            navAgent.destination = destination;
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

    //Run this code when player is spotted during low readiness alert state.
    float alertDelayCounter = 0f;
    float alertDuration = 0.5f;
    private void ObserveTarget()
    {
        print("observing!");
        if (seenTarget == null) {currentState = defaultState; return;}
        navAgent.SetDestination(transform.position);
        if (patrollingEnemyAlertController.GetHighReadinessStatus()) {currentState = ENEMYSTATES.CHASE; return;}
        if (patrollingEnemyAlertController.GetCurrentAlertBar() >= patrollingEnemyAlertController.GetCurrentMaxAlertBar())
        {currentState = ENEMYSTATES.CHASE; return;}

        var v3_Dir = seenTarget.position - transform.position;
        var v3_TargetRotation = Quaternion.LookRotation(v3_Dir);
        var flt_Lookspd = 12f;
        transform.rotation = Quaternion.Slerp(transform.rotation, v3_TargetRotation, Time.deltaTime * flt_Lookspd);
        
        float dist = (seenTarget.transform.position - transform.position).magnitude;
        if (dist <= 6f) { currentState = ENEMYSTATES.CHASE; return;}
        if (alertDelayCounter <= alertDuration)
        {
            alertDelayCounter += Time.deltaTime;
            print("counting!");
        }
        else
        {
            patrollingEnemyAlertController.AddAlertBar(7.5f);
            print("added value!");
            alertDelayCounter = 0f;
        }
    }

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
}
