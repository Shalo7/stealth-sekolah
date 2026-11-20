using UnityEngine;
using System.Collections;

public class SlidingGate : MonoBehaviour, IInteractable
{
    [SerializeField] Transform kiri;
    [SerializeField] Transform kanan;
    
    [SerializeField] float slideDist = 2f;
    [SerializeField] float moveDur = 2f;

    Vector3 kiriTutupPos;
    Vector3 kananTutupPos;

    Coroutine moveCoroutine;
    bool isOpen = false;

    void Awake()
    {
        kiriTutupPos = kiri.localPosition;
        kananTutupPos = kanan.localPosition;
    }
    public void Interact(PlayerPickup player)
    {
        isOpen = !isOpen;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveGate());
    }

    IEnumerator MoveGate()
    {
        Vector3 kiriMulai = kiri.localPosition;
        Vector3 kananMulai = kanan.localPosition;

        Vector3 kiriTarget;
        Vector3 kananTarget;

        if (isOpen)
        {
            kiriTarget = kiriTutupPos + Vector3.forward * slideDist;
            kananTarget = kananTutupPos + Vector3.back * slideDist;
        }
        else
        {
            kiriTarget = kiriTutupPos;
            kananTarget = kananTutupPos;
        }

        float elapsed = 0f;

        while (elapsed < moveDur)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / moveDur);

            t = Mathf.SmoothStep(0f, 1f, t);

            kiri.localPosition = Vector3.Lerp(kiriMulai, kiriTarget, t);
            kanan.localPosition = Vector3.Lerp(kananMulai, kananTarget, t);

            yield return null;
        }

        kiri.localPosition = kiriTarget;
        kanan.localPosition = kananTarget;

        moveCoroutine = null;
    }
}
