
using UnityEngine;

public class PatrollingEnemyAlertController : MonoBehaviour
{
    [SerializeField] float drainDelay = 1f;
    [SerializeField] float drainValue = 2.5f;
    [SerializeField] float currentAlertBar = 0f;
    public float GetCurrentAlertBar() => currentAlertBar;
    [SerializeField] float maxAlertBar = 100f;
    public float GetCurrentMaxAlertBar() => maxAlertBar;
    bool isHighReadiness = false;
    public bool GetHighReadinessStatus() => isHighReadiness;

    [SerializeField] PatrollingEnemyLogic patrollingEnemyLogic;

    void Start()
    {
        patrollingEnemyLogic = transform.parent.TryGetComponent(out PatrollingEnemyLogic c) ? c : null;
        print (patrollingEnemyLogic);  
    }

    private float drainCounter = 0f;
    //Update is called once per frame. checks if it has reference to enemy logic. if so, check if the alertbar is less or same as zero. if not, checks if the enemy state permits draining the alert bar. if yes, drain it at a set rate.
    void Update()
    {
        if (patrollingEnemyLogic == null) return;
        if (currentAlertBar <= 0f) return;
        if (patrollingEnemyLogic.GetCurrentEnemyState() == ENEMYSTATES.INVESTIGATEWALK || patrollingEnemyLogic.GetCurrentEnemyState() == ENEMYSTATES.INVESTIGATERUN || patrollingEnemyLogic.GetCurrentEnemyState() == ENEMYSTATES.CHASE || patrollingEnemyLogic.GetCurrentEnemyState() == ENEMYSTATES.OBSERVING) return;

        if (drainCounter <= drainDelay) {drainCounter += Time.deltaTime; return;}
        currentAlertBar -= drainValue;
        drainCounter = 0;
        if (currentAlertBar <= 0f) {isHighReadiness = false;}
    }

    public void AddAlertBar(float val)
    {
        currentAlertBar += val;
        if (currentAlertBar >= maxAlertBar)
        {
            currentAlertBar = maxAlertBar;
            isHighReadiness = true;
        }
        else if (currentAlertBar <= 0f)
        {
            currentAlertBar = 0f;
            isHighReadiness = false;
        }
    }
}
