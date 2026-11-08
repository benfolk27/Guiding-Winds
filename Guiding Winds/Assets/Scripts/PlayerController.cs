using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    private float playerSpeed = 5.0f;
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;
    private float sprintModifier = 2f;
    public float mouseSensitivity = 15f;

    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    
    public Transform cameraTransform;
    

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference sprintAction;
    public InputActionReference lookAction;
    
    
    [Header("Camera")]
    public GameObject camTarget;
    public float upClamp = 70.0f;
    public float downClamp = -30.0f;
    private float cmTargetYaw;
    private float cmTargetPitch;

    private void Start()
    {
        cmTargetYaw = camTarget.transform.rotation.eulerAngles.y;
        cmTargetPitch = camTarget.transform.rotation.eulerAngles.x;
    }
    
    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
        lookAction.action.Disable();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
        {
            // Slight downward velocity to keep grounded stable
            if (playerVelocity.y < -2f)
                playerVelocity.y = -2f;
        }

        // Read input
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        
        // look at camera direction
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // final move vector
        Vector3 move = forward * input.x + forward * input.y;
        move = Vector3.ClampMagnitude(move, 1f);

        if (move != Vector3.zero)
            transform.forward = move;

        // Jump using WasPressedThisFrame()
        if (groundedPlayer && jumpAction.action.WasPressedThisFrame())
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        float modifier = 1f;
        if (sprintAction.action.IsPressed())
        {
            modifier = sprintModifier;
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Move
        Vector3 finalMove = move * playerSpeed * modifier + Vector3.up * playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }
    
    private void CameraRotation()
    {
        Vector2 lookVector = lookAction.action.ReadValue<Vector2>();
        cmTargetYaw += lookVector.x * mouseSensitivity * Time.deltaTime;
        cmTargetPitch += lookVector.y * mouseSensitivity * Time.deltaTime;

        // clamp rotations so our values are limited 360 degrees
        cmTargetYaw = ClampAngle(cmTargetYaw, float.MinValue, float.MaxValue);
        cmTargetPitch = ClampAngle(cmTargetPitch, downClamp, upClamp);

        // Cinemachine will follow this target
        camTarget.transform.rotation = Quaternion.Euler(cmTargetPitch, cmTargetYaw, 0.0f);
    }
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}