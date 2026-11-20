using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    public Slider slider;
    public float max = 5f;
    
    void Start()
    {
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
