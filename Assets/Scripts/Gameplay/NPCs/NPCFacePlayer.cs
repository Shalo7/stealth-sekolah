using System.Collections;
using UnityEngine;

public class NPCFacePlayer : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform player;

    [Header("Detection")]
    [SerializeField] float detectionRadius = 5f;

    [Header("Rotation")]
    [SerializeField] float rotationSpeed = 5f;

    void Update()
    {
        if (player == null) return;

        if (IsPlayerInRange())
        {
            FacePlayer();
            //Debug.Log("Player Found");
        }
    }

    private bool IsPlayerInRange()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        return directionToPlayer.magnitude <= detectionRadius;
    }

    private void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        //Debug.DrawRay(transform.position, direction * 3f, Color.red);
        if (direction.sqrMagnitude > 0.001f)
        {
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
            {
                transform.rotation = targetRotation;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);   
            }
        }   
        //if (CO_FacePlayer != null) return;
        //CO_FacePlayer = StartCoroutine(IE_FacePlayer());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
        Gizmos.DrawSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void FacePlayerImmediately()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
