using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Guiding_Winds.Scripts.Player;
using UnityEngine.Windows;
using System;

namespace Assets.Guiding_Winds.Scripts.Systems.Quests
{
    public class QuestManager : MonoBehaviour
    {   
        [Header("Config")]
        [SerializeField] private bool loadQuestState = true;

        private RepStatus CurrentPlayerRepStatus;

        private Dictionary<string, Quest> QuestMap;

        private void Awake()
        {
            QuestMap = CreateQuestMap();
        }

        private void OnEnable()
        {
            GameEventsManager.Instance.QuestEvents.OnQuestStarted += StartQuest;
            GameEventsManager.Instance.QuestEvents.OnQuestAdvance += AdvanceQuest;
            GameEventsManager.Instance.QuestEvents.OnFinishQuest += FinishQuest;

            GameEventsManager.Instance.PlayerEvents.OnRepStatusChange += RepStatusChange;

            GameEventsManager.Instance.QuestEvents.OnQuestStepStateChange += QuestStepStateChange;
        }
        private void OnDisable()
        {
            GameEventsManager.Instance.QuestEvents.OnQuestStarted -= StartQuest;
            GameEventsManager.Instance.QuestEvents.OnQuestAdvance -= AdvanceQuest;
            GameEventsManager.Instance.QuestEvents.OnFinishQuest -= FinishQuest;
            GameEventsManager.Instance.PlayerEvents.OnRepStatusChange -= RepStatusChange;
            GameEventsManager.Instance.QuestEvents.OnQuestStepStateChange -= QuestStepStateChange;
        }

        private void RepStatusChange(RepStatus status)
        {
            CurrentPlayerRepStatus = status;
        }

        private Quest GetQuestById(string id) => QuestMap[id];

        private void StartQuest(string questId)
        {
            Quest quest = GetQuestById(questId);
            quest.InstantiateCurrentQuestStep(this.transform);
            ChangeQuestState(quest.info.Id, QuestState.IN_PROGRESS);
        }

        private void AdvanceQuest(string questId)
        {
            Quest quest = GetQuestById(questId);
            quest.MoveToNextStep();
            if (quest.CurrentStepExists())
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            else
            {
                ChangeQuestState(quest.info.Id, QuestState.CAN_FINISH);
            }
        }

        private void FinishQuest(string questId)
        {
            Quest quest = GetQuestById(questId);
            ClaimRewards(quest);
            ChangeQuestState(quest.info.Id, QuestState.FINISHED);
        }

        private void ClaimRewards(Quest quest)
        {
            GameEventsManager.Instance.PlayerEvents.RepGained(quest.info.RepReward);
        }

        public bool IsQuestActive(QuestInfo questInfo)
        {
            return questInfo != null
                && QuestMap.TryGetValue(questInfo.Id, out Quest quest)
                && quest.State == QuestState.IN_PROGRESS;
        }

        private void Start()
        {
            // Show all initial state of quests
            foreach (var quest in QuestMap.Values)
            {
                if (quest.State == QuestState.IN_PROGRESS)
                {
                   quest.InstantiateCurrentQuestStep(transform);
                }
                GameEventsManager.Instance.QuestEvents.QuestStateChange(quest);
            }
        }

        private void Update()
        {
            // loop through ALL quests
            foreach (Quest quest in QuestMap.Values)
            {
                // if we're now meeting the requirements, switch over to the CAN_START state
                if (quest.State == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
                {
                    ChangeQuestState(quest.info.Id, QuestState.CAN_START);
                }
            }
        }

        private bool CheckRequirementsMet(Quest quest)
        {
            // start true and prove to be false
            bool meetsRequirements = true;

            // check player reputation requirements
            if (CurrentPlayerRepStatus < quest.info.RepStatus)
            {
                meetsRequirements = false;
            }

            // check quest prerequisites for completion
            foreach (QuestInfo prerequisiteQuestInfo in quest.info.PreRequisiteQuests)
            {
                if (GetQuestById(prerequisiteQuestInfo.Id).State != QuestState.FINISHED)
                {
                    meetsRequirements = false;
                }
            }

            return meetsRequirements;
        }

        private void ChangeQuestState(string id, QuestState state)
        {
            Quest quest = GetQuestById(id);
            quest.State = state;
            GameEventsManager.Instance.QuestEvents.QuestStateChange(quest);
        }

        private Dictionary<string, Quest> CreateQuestMap()
        {
            // loads all QuestInfo Scriptable Objects under the Assets/Resources/Quests folder
            QuestInfo[] allQuests = Resources.LoadAll<QuestInfo>("Quests");
            // Create the quest map
            Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
            foreach (QuestInfo questInfo in allQuests)
            {
                if (idToQuestMap.ContainsKey(questInfo.Id))
                {
                    Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.Id);
                }
                idToQuestMap.Add(questInfo.Id, LoadQuest(questInfo));
            }
            return idToQuestMap;
        }

        private void QuestStepStateChange(string questId, int stepIndex, QuestStepState state)
        {
            Quest quest = GetQuestById(questId);
            quest.StoreQuestStepState(stepIndex, state);
            ChangeQuestState(questId, quest.State);
        }

        private void OnApplicationQuit()
        {
            foreach (var quest in QuestMap.Values)
            {
                QuestData questData = quest.GetQuestData();
                Debug.Log("State of quest " + quest.info.Id + ": " + questData.State);
                //SaveQuest(quest);
            }
        }

        private void SaveQuest(Quest quest)
        {
            try 
            {
                QuestData questData = quest.GetQuestData();
                // serialize using JsonUtility, but use whatever you want here (like JSON.NET)
                string serializedData = JsonUtility.ToJson(questData);
                // saving to PlayerPrefs is just a quick example for this tutorial video,
                // you probably don't want to save this info there long-term.
                // instead, use an actual Save & Load system and write to a file, the cloud, etc..
                PlayerPrefs.SetString(quest.info.Id, serializedData);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to save quest with id " + quest.info.Id + ": " + e);
            }
        }

        private Quest LoadQuest(QuestInfo questInfo)
        {
            Quest quest = null;
            try 
            {
                // load quest from saved data
                if (PlayerPrefs.HasKey(questInfo.Id) && loadQuestState)
                {
                    string serializedData = PlayerPrefs.GetString(questInfo.Id);
                    QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                    quest = new Quest(questInfo, questData.State, questData.QuestStepIndex, questData.QuestStepStates);
                }
                // otherwise, initialize a new quest
                else 
                {
                    quest = new Quest(questInfo);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load quest with id " + quest.info.Id + ": " + e);
            }
            return quest;
        }
    }
}