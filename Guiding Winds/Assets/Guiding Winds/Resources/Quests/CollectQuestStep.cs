using Assets.Guiding_Winds.Scripts.Systems;
using Assets.Guiding_Winds.Scripts.Systems.Quest;
using UnityEngine;

public class CollectQuestStep : QuestStep
{
    [SerializeField] private QuestObject RequiredObject;
    [SerializeField, Min(1)] private int RequiredCount = 1;

    private int CollectedCount;

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.OnObjectCollected += ObjectCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.OnObjectCollected -= ObjectCollected;
    }

    private void ObjectCollected(QuestObject CollectedObject)
    {
        if (RequiredObject == null || CollectedObject != RequiredObject)
            return;

        CollectedCount++;

        if (CollectedCount >= RequiredCount)
            FinishQuestStep();
    }
}