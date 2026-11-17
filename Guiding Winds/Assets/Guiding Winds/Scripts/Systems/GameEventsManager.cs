using UnityEngine;
using Assets.Guiding_Winds.Scripts.Systems.Quests;

namespace Assets.Guiding_Winds.Scripts.Systems
{
    public class GameEventsManager : MonoBehaviour
    {
        public static GameEventsManager Instance { get; private set; }

        public QuestEvents QuestEvents;
        public InputEvents InputEvents;
        public PlayerEvents PlayerEvents;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one Game Events Manager in the scene.");
            }
            Instance = this;
            QuestEvents = new QuestEvents();
            InputEvents = new InputEvents();
            PlayerEvents = new PlayerEvents();
        }
    }
}