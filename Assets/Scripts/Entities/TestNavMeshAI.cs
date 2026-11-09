using System;
using UnityEngine;
using UnityEngine.AI;

public class TestNavMeshAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] bool active;
    [SerializeField] Transform destination;

    void Update()
    {
        if (!active) return;
        Debug.Log("active!");
        if (navMeshAgent == null) return;
        Debug.Log("has nav mesh!");
        //if (!navMeshAgent.pathPending) return;
        Debug.Log("path is pending!");
        //if (!navMeshAgent.hasPath) return;
        Debug.Log("has path!");

        navMeshAgent.SetDestination(destination.position);
        float range = (destination.position - transform.position).sqrMagnitude;

        if (range <= Mathf.Abs(0.001f)) { active = false; }
    }
}
