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

        public Quest(QuestInfo questInfo)
        {
            info = questInfo;
            State = QuestState.REQUIREMENTS_NOT_MET;
            CurrentQuestStepIndex = 0;
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
                    .InitializeQuestStep(info.Id);
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
    }
}