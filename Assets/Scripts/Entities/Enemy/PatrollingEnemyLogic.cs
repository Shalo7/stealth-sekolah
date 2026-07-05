using System;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingEnemyLogic : MonoBehaviour
{
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] PlayerMarker plr;
    [SerializeField] LayerMask groundLayer, playerLayer;
    [SerializeField] float walkPointRange;
    [SerializeField] Animator animator;
    [SerializeField] PatrollingEnemyEventComms patrollingEnemyEventComms;
    

    private Vector3 walkPoint;
    private bool walkPointSet;

    void OnEnable()
    {
        patrollingEnemyEventComms.playerSightedComms += playerSightedReceiver;
    }

    void Start()
    {
        plr = PlayerMarker.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void playerSightedReceiver(Transform target)
    {
        if (target != null)
        {
            navAgent.SetDestination(plr.transform.position);
            navAgent.isStopped = false;
        }
        else
        {
            navAgent.SetDestination(transform.position);
        }
    }
}
