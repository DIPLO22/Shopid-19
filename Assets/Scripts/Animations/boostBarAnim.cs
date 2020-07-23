using UnityEngine.UI;
using UnityEngine;

public class boostBarAnim : MonoBehaviour
{
    public Slider slider;
    private Image fill;
    private RectTransform rt;
    void Start()
    {
        fill = GetComponent<Image>();
        rt = slider.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    public void jumpAnim(float boost, bool isJumping)
    {
        slider.value = boost;
        
        if(isJumping)
            rt.sizeDelta = Vector2.Lerp(rt.sizeDelta, new Vector2(220,45), 0.3f);

        else
            rt.sizeDelta = Vector2.Lerp(rt.sizeDelta, new Vector2(200,25), 0.3f);
    }
}
