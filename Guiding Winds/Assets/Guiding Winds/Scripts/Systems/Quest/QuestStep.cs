using System.Collections;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Systems.Quests
{
    public abstract class QuestStep : MonoBehaviour
    {

        private bool IsFinished = false;
        private int StepIndex;
        private string QuestId;

        public void InitializeQuestStep(string questId, int stepIndex, string QuestStepState)
        {
            QuestId = questId;
            StepIndex = stepIndex;
            if (QuestStepState != null && QuestStepState != "")
            {
                SetQuestStepState(QuestStepState);
            }
        }

        protected void FinishQuestStep()
        {
            if (!IsFinished)
            {
                IsFinished = true;
                GameEventsManager.Instance.QuestEvents.QuestAdvance(QuestId);
                Destroy(gameObject);
            }
        }

        protected void ChangeState(string NewState)
        {
            GameEventsManager.Instance.QuestEvents.QuestStepStateChange(QuestId, StepIndex, new QuestStepState(NewState));
        }

        protected abstract void SetQuestStepState(string state);
    }
}