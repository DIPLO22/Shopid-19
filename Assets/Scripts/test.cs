using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public float maxHealth = 100, health, dps = 25;
    public Gradient grad;
    public Slider slider;
    public Image fill;
    private RectTransform rt;
    private bool isHurt;

    void Start()
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        health = maxHealth;
        rt = slider.GetComponent<RectTransform>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            isHurt = true;
        }

        else
        {
            isHurt = false;
        }
        doSlider();
    }

    void doSlider()
    {
        if(isHurt && health>0)
        {
            health -= dps * Time.deltaTime;
            rt.sizeDelta = Vector2.Lerp(rt.sizeDelta, new Vector2(220,45), 0.3f);
        }
        else
            rt.sizeDelta = Vector2.Lerp(rt.sizeDelta, new Vector2(200,25), 0.3f);

        Mathf.Clamp(health,0,maxHealth);

        fill.color = grad.Evaluate(health/maxHealth);
        slider.value = health;
    }
}
