using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public float max = 5f;
    
    void Start()
    {
        slider.maxValue = max;
        slider.value = max;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void change(float value)
    {
        slider.value += value;
    }
}
