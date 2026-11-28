using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class CentaurTransformation : MonoBehaviour
{
    public Slider slider;
    public Slider timerSlider;
    public Slider staminaSlider;
    public float max = 100f;
    public float transformRate = 30f;
    public float untransformRate = 10f;
    public InputActionReference transform;
    public bool isTransformed = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.action.Enable();
        slider.maxValue = max;
        slider.value = 0f;
        timerSlider.maxValue = max;
        timerSlider.value = max;
        slider.gameObject.SetActive(false);
        timerSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransformed)
        {
            if (transform.action.IsPressed())
            {
                slider.gameObject.SetActive(true);
                slider.value += transformRate * Time.deltaTime;
                if (slider.value >= max)
                {
                    slider.gameObject.SetActive(false);
                    slider.value = 0f;
                    
                    timerSlider.gameObject.SetActive(true);
                    timerSlider.value = max;
                    isTransformed = true;
                }
            }
            else
            {
                slider.gameObject.SetActive(false);
                slider.value = 0f;
            }
        }
        else
        {
            staminaSlider.value = max;
            timerSlider.value -= untransformRate * Time.deltaTime;
            if (timerSlider.value <= 0f)
            {
                isTransformed = false;
                timerSlider.gameObject.SetActive(false);
            }
        }
    }
}
