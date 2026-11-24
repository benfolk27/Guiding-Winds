using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Guiding_Winds.Scripts.Player;
using UnityEngine.Windows;

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
        }
        private void OnDisable()
        {
            GameEventsManager.Instance.QuestEvents.OnQuestStarted -= StartQuest;
            GameEventsManager.Instance.QuestEvents.OnQuestAdvance -= AdvanceQuest;
            GameEventsManager.Instance.QuestEvents.OnFinishQuest -= FinishQuest;
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
            quest.InstantiateCurrentQuestStep(this.transform);
        }

        private void FinishQuest(string questId)
        {
            Quest quest = GetQuestById(questId);
            ChangeQuestState(quest.info.Id, QuestState.FINISHED);
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
            QuestInfo[] allQuests = Resources.LoadAll<QuestInfo>("Quests");
            Dictionary<string, Quest> IdToQuestMap = new();
            foreach (QuestInfo questInfo in allQuests)
            {
                if (IdToQuestMap.ContainsKey(questInfo.Id))
                {
                    Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.Id);
                }
                IdToQuestMap.Add(questInfo.Id, LoadQuest(questInfo));
            }
            return IdToQuestMap;
        }

        private Quest LoadQuest(QuestInfo questInfo)
        {
            Quest quest = null;
            try
            {
                if (PlayerPrefs.HasKey(questInfo.Id))
                {
                    string serializedData = PlayerPrefs.GetString(questInfo.Id);
                }
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