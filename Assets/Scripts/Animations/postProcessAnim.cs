using UnityEngine.Rendering;
using UnityEngine;

public class postProcessAnim : MonoBehaviour
{
    Volume volume;
    
    void Start()
    {
        healthScript.whenInfected += infectionPostProcess;
        volume = GetComponent<Volume>();
    }
    public void infectionPostProcess(float health)
    {
        volume.weight = utils.map(health,20,60,1,0);
    }
}
