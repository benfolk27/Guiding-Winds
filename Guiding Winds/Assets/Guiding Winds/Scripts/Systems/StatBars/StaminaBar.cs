using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public float max = 100f;
    public float border = 2f;
    public float sprintRate = 10f;
    public float recoverRate = 5f;
    public bool canSprint = true;
    public InputAction sprint;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprint.Enable();
        slider.maxValue = max;
        slider.value = max;
    }

    // Update is called once per frame
    void Update()
    {
        if (sprint.IsPressed() && canSprint)
        {
            slider.value -= sprintRate * Time.deltaTime;
            if (slider.value <= 0)
            {
                slider.value = 0;
                canSprint = false;
            }
        }
        else
        {
            slider.value += recoverRate * Time.deltaTime;
            if (slider.value >= border)
            {
                canSprint = true;
            }
        }
    }
    
    public void decrement(float value)
    {
        slider.value -= value;
    }
}
