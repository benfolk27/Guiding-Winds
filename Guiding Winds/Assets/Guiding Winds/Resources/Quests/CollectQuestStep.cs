using Assets.Guiding_Winds.Scripts.Systems;
using Assets.Guiding_Winds.Scripts.Systems.Quests;
using UnityEngine;

public class CollectQuestStep : QuestStep
{
    [SerializeField] private QuestObject RequiredObject;
    [SerializeField, Min(1)] private int RequiredCount = 1;

    private int CollectedCount;

    private void OnEnable()
    {
        GameEventsManager.Instance.QuestEvents.OnObjectCollected += ObjectCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnObjectCollected -= ObjectCollected;
    }

    private void ObjectCollected(QuestObject CollectedObject)
    {
        if (RequiredObject == null || CollectedObject != RequiredObject)
            return;

        UpdateState();
        CollectedCount++;

        if (CollectedCount >= RequiredCount)
            FinishQuestStep();
    }

    private void UpdateState()
    {
        string state = CollectedCount.ToString();
        base.ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        CollectedCount = int.Parse(state);
        UpdateState();
    }
}