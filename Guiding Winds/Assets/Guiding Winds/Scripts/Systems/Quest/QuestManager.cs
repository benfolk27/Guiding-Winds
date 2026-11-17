using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Assets.Guiding_Winds.Scripts.Systems.Quests
{
    public class QuestManager : MonoBehaviour
    {

        private Dictionary<string, Quest> QuestMap;

        private void Awake()
        {
            QuestMap = CreateQuestMap();
        }

        private void OnEnable()
        {
            GameEventsManager.Instance.QuestEvents.OnQuestStarted += StartQuest;
            GameEventsManager.Instance.QuestEvents.OnQuestAdvance += AdvanceQuest;
            GameEventsManager.Instance.QuestEvents.OnQuestCompleted += CompleteQuest;
        }
        private void OnDisable()
        {
            GameEventsManager.Instance.QuestEvents.OnQuestStarted -= StartQuest;
            GameEventsManager.Instance.QuestEvents.OnQuestAdvance -= AdvanceQuest;
            GameEventsManager.Instance.QuestEvents.OnQuestCompleted -= CompleteQuest;
        }

        // For debug only
        //private Quest GetQuestById(string id) => QuestMap[id];

        private void StartQuest(string questId)
        {
            
        }

        private void AdvanceQuest(string questId)
        {
            
        }

        private void CompleteQuest(string questId)
        {
            
        }

        private void Start()
        {
            // Show all initial state of quests
            foreach (var quest in QuestMap.Values)
            {
                GameEventsManager.Instance.QuestEvents.QuestStateChange(quest);
            }
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