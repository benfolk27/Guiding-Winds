using System;

namespace Assets.Guiding_Winds.Scripts.Systems.Quests
{
    public class QuestEvents
    {
        // Quest lifecycle events
        public event Action<string> OnQuestStarted;

        public void QuestStarted(string questId)
        {
            OnQuestStarted?.Invoke(questId);
        }
        
        public event Action<string> OnQuestAdvance;

        public void QuestAdvance(string questId)
        {
            OnQuestAdvance?.Invoke(questId);
        }
        
        public event Action<string> OnQuestCompleted;

        public void QuestCompleted(string questId)
        {
            OnQuestCompleted?.Invoke(questId);
        }

        public event Action<string> OnFinishQuest;

        public void FinishQuest(string id)
        {
            OnFinishQuest?.Invoke(id);
        }

        // Quest state events
        public event Action<Quest> OnQuestStateChange;

        public void QuestStateChange(Quest quest)
        {
            OnQuestStateChange?.Invoke(quest);
        }

        // Quest objective events
        public event Action<QuestObject> OnObjectCollected;

        public void ObjectCollected(QuestObject questObject)
        {
            OnObjectCollected?.Invoke(questObject);
        }
    }
}