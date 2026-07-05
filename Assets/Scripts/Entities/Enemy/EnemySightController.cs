using System;
using UnityEngine;

public class EnemySightController : MonoBehaviour
{
    [SerializeField] PlayerMarker plr;
    [SerializeField] float visionDot;
    [SerializeField] float minDist;
    Action<Transform> playerSighted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plr = PlayerMarker.instance;
    }

    // Update is called once per frame
    void Update()
    {
        SightSystem();
    }

    //Enemy gets the player's location before checking its distance and whether or not they are within field of view. If both of these conditions are met then check whether or not it can see the player.
    private void SightSystem()
    {
        //If the player marker does not exist then don't run this code.
        if (plr == null) return;
        //print("Player exists!");
        
        //Get the necessary data for the enemy sight.
        float plrDist = (plr.transform.position - transform.position).magnitude;
        Vector3 dir = (plr.transform.position - transform.position).normalized;
        var flt_DotToPlayer = Vector3.Dot(dir, transform.forward);
        print(flt_DotToPlayer);
        Debug.DrawRay(transform.position, transform.forward * 3f, Color.blue);
        Debug.DrawRay(transform.position, dir * 3f, Color.red);

        //Check if the player is within range.
        if (minDist < plrDist) return;

        //print("Player within range!");

        //Check if the player is within field of view
        if (flt_DotToPlayer < visionDot) return;

        // Check if the player is not behind any object to be seen clearly.
        print("Player within field of view!");
        RaycastHit hit;
        bool castCheck = Physics.Raycast(transform.position, dir, out hit);
        if (!hit.transform.TryGetComponent<PlayerMarker>(out PlayerMarker pm)) return;
        print($"Player is seen and their name is {pm.transform.name}");
    }
}
