using Assets.Guiding_Winds.Scripts.Systems;
using Assets.Guiding_Winds.Scripts.Systems.Quest;
using UnityEngine;

public class QuestPickup : MonoBehaviour
{
    [SerializeField] private QuestObject QuestObject;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || QuestObject == null)
            return;

        GameEventsManager.Instance.questEvents.ObjectCollected(QuestObject);
        Destroy(gameObject);
    }

    //How to create pickup item for quest:
    //    - Create quest object scriptable object in unity (usually under whatever quest folder ur triyng to do
    //    - Add "Quest Pickup" script to actual 3D object needed and add that quest object scriptable object we made
    //    - In the Quest Step prefab, under the quest step script, add that quest object scriptable object we made again
    //    - Make sure quest step prefab is under the quest info scriptable object
}