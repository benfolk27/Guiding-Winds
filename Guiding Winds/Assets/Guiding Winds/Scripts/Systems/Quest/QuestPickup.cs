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
}