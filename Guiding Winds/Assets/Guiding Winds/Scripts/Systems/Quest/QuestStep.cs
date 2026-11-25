using System.Collections;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Systems.Quests
{
    public abstract class QuestStep : MonoBehaviour
    {

        private bool IsFinished = false;
        private string QuestId;

        public void InitializeQuestStep(string questId)
        {
            QuestId = questId;
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
    }
}