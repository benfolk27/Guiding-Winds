using System.Collections;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Systems.Quest
{
    public abstract class QuestStep : MonoBehaviour
    {

        private bool IsFinished = false;

        protected void FinishQuestStep()
        {
            if (!IsFinished)
            {
                IsFinished = true;

                // send event to move onto next step

                Destroy(gameObject);
            }
        }
    }
}