using Assets.Guiding_Winds.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Systems.Quest
{
    [CreateAssetMenu(fileName = "QuestInfo", menuName = "ScriptableObjects/QuestInfo", order = 1)]
    public class QuestInfo : ScriptableObject
    {

        [field: SerializeField]
        public string Id { get; private set; }

        [Header("General")]
        public string DisplayName;

        [Header("Requirements")]
        public QuestInfo[] PreRequisiteQuests;
        public RepStatus RepStatus;

        [Header("Steps")]
        public GameObject[] QuestStepPrefabs;

        [Header("Rewards")]
        public int repReward;

        private void OnValidate()
        {
        #if UNITY_EDITOR
            Id = name;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
        }
    }
}