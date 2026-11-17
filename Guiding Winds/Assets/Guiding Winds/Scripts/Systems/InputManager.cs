using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Guiding_Winds.Scripts.Systems
{
    // bc we are using unity third person controller for input
    [RequireComponent(typeof(PlayerInput), typeof(StarterAssetsInputs))]
    public class InputManager : MonoBehaviour
    {
        private StarterAssetsInputs starterInputs;

        private void Awake()
        {
            starterInputs = GetComponent<StarterAssetsInputs>();
        }

        private void OnEnable()
        {
            starterInputs.OnMoveEvent += MovePressed;
        }

        private void OnDisable()
        {
            starterInputs.OnMoveEvent -= MovePressed;
        }

        private void MovePressed(Vector2 movement)
        {
            Debug.Log("Move pressed: " + movement);
            GameEventsManager.Instance.InputEvents.MovePressed(movement);
        }

        public void OnSubmit(InputValue value)
        {
            if (value.isPressed)
                Debug.Log("Submit pressed");
                GameEventsManager.Instance.InputEvents.SubmitPressed();
        }

        public void OnQuestLogToggle(InputValue value)
        {
            if (value.isPressed)
                Debug.Log("Quest Log Toggle pressed");
                GameEventsManager.Instance.InputEvents.QuestLogTogglePressed();
        }
    }
}