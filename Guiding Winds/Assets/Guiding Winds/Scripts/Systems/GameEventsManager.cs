using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Systems
{
    public class GameEventsManager : MonoBehaviour
    {
        public static GameEventsManager Instance { get; private set; }

        public QuestEvents questEvents;

        public InputEvents inputEvents;
        public PlayerEvents playerEvents;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one Game Events Manager in the scene.");
            }
            Instance = this;
            questEvents = new QuestEvents();
            inputEvents = new InputEvents();
            playerEvents = new PlayerEvents();
        }
    }
}