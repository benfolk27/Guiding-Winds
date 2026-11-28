using Assets.Guiding_Winds.Scripts.Systems.Quests;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Systems.Quests
{
    public class Quest
    {
        // static info
        public QuestInfo info;

        // dynamic state
        public QuestState State;
        private int CurrentQuestStepIndex;
        private QuestStepState[] QuestStepStates;

        public Quest(QuestInfo questInfo)
        {
            info = questInfo;
            State = QuestState.REQUIREMENTS_NOT_MET;
            CurrentQuestStepIndex = 0;
            QuestStepStates = new QuestStepState[questInfo.QuestStepPrefabs.Length];
            for (int i = 0; i < QuestStepStates.Length; i++)
            {
                QuestStepStates[i] = new QuestStepState();
            }
        }

        public Quest(QuestInfo questInfo, QuestState QuestState, int CurrentQuestStepIndex, QuestStepState[] QuestStepStates)
        {
            info = questInfo;
            State = QuestState;
            this.CurrentQuestStepIndex = CurrentQuestStepIndex;
            this.QuestStepStates = QuestStepStates;

            // if the quest step states and prefabs are different lengths,
            // something has changed during development and the saved data is out of sync.
            if (this.QuestStepStates.Length != this.info.QuestStepPrefabs.Length)
            {
                Debug.LogWarning("Quest Step Prefabs and Quest Step States are "
                    + "of different lengths. This indicates something changed "
                    + "with the QuestInfo and the saved data is now out of sync. "
                    + "Reset your data - as this might cause issues. QuestId: " + this.info.Id);
            }
        }

        public void MoveToNextStep()
        {
            CurrentQuestStepIndex++;
        }

        public bool CurrentStepExists() => CurrentQuestStepIndex < info.QuestStepPrefabs.Length;

        public void InstantiateCurrentQuestStep(Transform parentTransform)
        {
            GameObject prefab = GetCurrentQuestStepPrefab();
            if (prefab != null)
            {
                GameObject.Instantiate(prefab, parentTransform)
                    .GetComponent<QuestStep>()
                    .InitializeQuestStep(info.Id, CurrentQuestStepIndex, QuestStepStates[CurrentQuestStepIndex].state);
            }
        }

        private GameObject GetCurrentQuestStepPrefab()
        {
            if (CurrentStepExists())
            {
                return info.QuestStepPrefabs[CurrentQuestStepIndex];
            }
            return null;
        }
        public void StoreQuestStepState(int stepIndex, QuestStepState state)
        {
            if (stepIndex < QuestStepStates.Length)
            {
                QuestStepStates[stepIndex].state = state.state;
            }
        }
        public QuestData GetQuestData() => new(info.Id, State, CurrentQuestStepIndex, QuestStepStates);
    }
}