using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Systems.Quests
{
    [System.Serializable]
    public class QuestData
    {
        public string QuestId;
        public QuestState State;
        public int QuestStepIndex;
        public QuestStepState[] QuestStepStates;

        public QuestData(string questId, QuestState state, int questStepIndex, QuestStepState[] questStepStates)
        {
            QuestId = questId;
            QuestStepIndex = questStepIndex;
            State = state;
            QuestStepStates = questStepStates;
        }
    }
}