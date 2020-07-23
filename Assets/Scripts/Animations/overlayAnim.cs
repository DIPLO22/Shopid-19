using UnityEngine.UI;
using UnityEngine;

public class overlayAnim : MonoBehaviour
{
    public float maxPulse = 20, minPulse = 10;
    private Color startColor;
    private Image img;
    private float currentPulse, t;

    void Start()
    {
        img = GetComponent<Image>();
        startColor = img.color;
    }

    void Update()
    {
        if(healthScript.playerHealth.health<35)
            animate(healthScript.playerHealth.health);
    }

    public void animate(float health)
    {
        currentPulse = utils.map(health,0,30,maxPulse,minPulse);
        Color tempColor = startColor;
        t += Time.deltaTime * currentPulse;
        tempColor.a = utils.map(Mathf.Sin(t),-1,1,0,1);
        GetComponent<Image>().color = tempColor;
        
        if(img.color != startColor && health>30)
        {
            img.color = startColor;
            t = 0;
        }
    }
}
