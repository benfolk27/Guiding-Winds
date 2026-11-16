using System;
using Assets.Guiding_Winds.Scripts.Systems.Quest;

namespace Assets.Guiding_Winds.Scripts.Systems
{
    public class QuestEvents
    {
        public event Action<QuestObject> OnObjectCollected;

        public void ObjectCollected(QuestObject questObject)
        {
            OnObjectCollected?.Invoke(questObject);
        }
    }
}