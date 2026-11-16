using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;


namespace Assets.Guiding_Winds.Scripts.Systems
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        // bc we are using unity third person controller for input
        private StarterAssetsInputs starterInputs;

        private void Awake()
        {
            starterInputs = GetComponent<StarterAssetsInputs>();
        }
        public void MovePressed(InputAction.CallbackContext context)
        {
            if (context.performed || context.canceled)
            {
                Vector2 movement = context.ReadValue<Vector2>();
                starterInputs.MoveInput(movement);
                GameEventsManager.Instance.inputEvents.MovePressed(movement);
            }
        }
    }
}