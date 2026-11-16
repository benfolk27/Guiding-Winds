using System;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Systems
{

    public class InputEvents
    {
        public event Action<Vector2> OnMove;
        public event Action OnSubmit;
        public event Action OnQuestLogToggle;

        public InputEventContext InputEventContext { get; private set; } = InputEventContext.DEFAULT;

        public void ChangeInputEventContext(InputEventContext newContext)
        {
            InputEventContext = newContext;
        }

        public void MovePressed(Vector2 value) => OnMove?.Invoke(value);
        public void SubmitPressed() => OnSubmit?.Invoke();
        public void QuestLogTogglePressed() => OnQuestLogToggle?.Invoke();
    }

    public enum InputEventContext
    {
        DEFAULT,
        DIALOGUE
    }
}