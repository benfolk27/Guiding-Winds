using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Assets.Guiding_Winds.Scripts.Systems.Quest
{
    public class QuestManager : MonoBehaviour
    {

        private Dictionary<string, Quest> QuestMap;

        private void Awake()
        {
            QuestMap = CreateQuestMap();
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