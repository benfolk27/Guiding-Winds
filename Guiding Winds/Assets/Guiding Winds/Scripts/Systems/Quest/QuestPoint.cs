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


        public void Awake()
        {
            QuestID = QuestInfo.Id;
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
            //if (PlayerInRange && CurrentQuestState == QuestState.CAN_START)
            if (PlayerInRange)
            {
                GameEventsManager.Instance.QuestEvents.QuestStarted(QuestID);
            }
        }
    }
}