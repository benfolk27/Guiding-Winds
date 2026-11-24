using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Systems.Quests
{   
    [RequireComponent(typeof(Collider))]
    public class QuestPoint : MonoBehaviour
    {
        [Header("Quest")]
        [SerializeField] private QuestInfo QuestInfo;

        private string QuestID;

        private QuestState CurrentQuestState;

        private QuestIcon Icon;

        [Header("Config")]
        [SerializeField] private bool StartPoint = true;
        [SerializeField] private bool FinishPoint = true;

        public void Awake()
        {
            QuestID = QuestInfo.Id;
            Icon = GetComponentInChildren<QuestIcon>();
        }

        public void OnEnable()
        {
            GameEventsManager.Instance.QuestEvents.OnQuestStateChange += QuestStateChange;
            GameEventsManager.Instance.InputEvents.OnSubmit += SubmitPressed;
        }
        public void OnDisable()
        {
            GameEventsManager.Instance.QuestEvents.OnQuestStateChange -= QuestStateChange;
            GameEventsManager.Instance.InputEvents.OnSubmit -= SubmitPressed;
        }
        
        
        private void QuestStateChange(Quest q)
        {
            if (q.info.Id != QuestID)
                return;

            CurrentQuestState = q.State;
            Icon.SetState(CurrentQuestState, StartPoint, FinishPoint);
        }

        private bool PlayerInRange = false;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            PlayerInRange = true;
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            PlayerInRange = false;
        }
        private void SubmitPressed()
        {
            if (PlayerInRange && CurrentQuestState == QuestState.CAN_START && StartPoint)
            {
                GameEventsManager.Instance.QuestEvents.QuestStarted(QuestID);
                Debug.Log("Quest started: " + QuestID);
            }
            else if (PlayerInRange && CurrentQuestState == QuestState.CAN_FINISH && FinishPoint)
            {
                GameEventsManager.Instance.QuestEvents.FinishQuest(QuestID);
                Debug.Log("Quest finished: " + QuestID);
            }
        }
    }
}