using Unity.VisualScripting;
using UnityEngine;

public class TestSoundMouseClick : MonoBehaviour
{
    [SerializeField] float soundStrength = 5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit h))
            {
                LayerMask ears = LayerMask.GetMask("EnemyEar");
                Collider[] earsCollided = Physics.OverlapSphere(h.point, soundStrength, ears);
                foreach(Collider col in earsCollided)
                {
                    if (!col.transform.TryGetComponent(out EnemyListenController listCont)) continue;
                    listCont.TriggerListen(h.point);
                    print(col.transform.name);
                }
            }
        }
    }
}
