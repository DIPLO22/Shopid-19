using UnityEngine.UI;
using UnityEngine;

public class healthBarAnim : MonoBehaviour
{
    public Gradient grad;
    public Slider slider;
    private Image fill;
    private RectTransform rt;
    void Start()
    {
        fill = GetComponent<Image>();
        rt = slider.GetComponent<RectTransform>();
        healthScript.whenInfected += manageSliderWhenInfected;
        healthScript.whenDoneInfected += manageSliderWhenDoneInfected;
    }

    // Update is called once per frame
    public void manageSliderWhenInfected(float health)
    {
        slider.value = health;
        fill.color = grad.Evaluate(health/100);
        
        rt.sizeDelta = Vector2.Lerp(rt.sizeDelta, new Vector2(220,45), 0.3f);
    }

    public void manageSliderWhenDoneInfected()
    {
        rt.sizeDelta = Vector2.Lerp(rt.sizeDelta, new Vector2(200,25), 0.3f);
    }
}
