using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SideDash : MonoBehaviour
{
    [Header("References")] 
    public Transform orientation;
    public InputActionReference sidedashAction;
    public CharacterController controller;
    public StaminaBar staminaBar;

    [Header("Dashing")] 
    public float dashSpeed = 18f;
    public float dashDuration = 0.2f;
    public bool isDashing = false;
    public float staminaCost = 20f;
    
    [Header("Cooldown")]
    public float dashCooldown = 0.08f;
    private float dashCdTimer;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sidedashAction.action.Enable();
        sidedashAction.action.performed += onSidedash;
    }

    // Update is called once per frame
    void Update()
    {
        if (dashCdTimer > 0f)
            dashCdTimer -= Time.deltaTime;
    }

    private void onSidedash(InputAction.CallbackContext context)
    {
        if (isDashing || dashCdTimer > 0f || staminaBar.slider.value < staminaCost) return;
        staminaBar.decrement(staminaCost);
        float sideDirection = 0f;
        if (Keyboard.current.aKey.isPressed)
        {
            sideDirection = -1f;
        }
        else
        {
            sideDirection = 1f;
        }
        StartCoroutine(PerformDash(sideDirection));
    }

    private IEnumerator PerformDash(float sideDirection)
    {
        isDashing = true;
        dashCdTimer = dashCooldown;

        Vector3 camDirection = orientation.right;
        camDirection.y = 0f;
        camDirection.Normalize();
        Vector3 dashDirection = camDirection * sideDirection;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }
}
