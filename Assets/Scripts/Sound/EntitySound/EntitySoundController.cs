using UnityEngine;

public class EntitySoundController : MonoBehaviour
{
    [SerializeField] float totalOverallSoundEnergy;
    [SerializeField] float maxOverallSoundEnergy;

    public void AddSoundEnergy(float val)
    {
        Debug.Log(val);
        if (totalOverallSoundEnergy >= maxOverallSoundEnergy) return;
        totalOverallSoundEnergy = val;
        if (totalOverallSoundEnergy >= maxOverallSoundEnergy) totalOverallSoundEnergy = maxOverallSoundEnergy; 
    }

    private void ProduceSoundEnergyRadius()
    {
        if (Time.frameCount % 10 == 0)
        {
            LayerMask ears = LayerMask.GetMask("EnemyEar");
            Collider[] earsCollided = Physics.OverlapSphere(transform.position, totalOverallSoundEnergy, ears);
            foreach(Collider col in earsCollided)
            {
                if (!col.transform.TryGetComponent(out EnemyListenController listCont)) continue;
                listCont.TriggerListen(transform.position);
                //print(col.transform.name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProduceSoundEnergyRadius();
    }
}
