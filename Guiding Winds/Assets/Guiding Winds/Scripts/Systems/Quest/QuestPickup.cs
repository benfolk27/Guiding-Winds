using Assets.Guiding_Winds.Scripts.Systems;
using UnityEngine;


namespace Assets.Guiding_Winds.Scripts.Systems.Quests {
    public class QuestPickup : MonoBehaviour
    {
        [SerializeField] private QuestObject QuestObject;
        
        [SerializeField] private QuestInfo RequiredQuest;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || QuestObject == null)
                return;

            QuestManager manager = FindFirstObjectByType<QuestManager>();
            if (manager == null || !manager.IsQuestActive(RequiredQuest))
                return;

            GameEventsManager.Instance.QuestEvents.ObjectCollected(QuestObject);
            Destroy(gameObject);
        }

        //How to create pickup item for quest:
        //    - Create quest object scriptable object in unity (usually under whatever quest folder ur triyng to do
        //    - Add "Quest Pickup" script to actual 3D object needed and add that quest object scriptable object we made
        //    - In the Quest Step prefab, under the quest step script, add that quest object scriptable object we made again
        //    - Make sure quest step prefab is under the quest info scriptable object
    }
}