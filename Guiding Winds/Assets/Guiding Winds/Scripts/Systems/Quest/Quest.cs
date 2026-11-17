using Assets.Guiding_Winds.Scripts.Systems.Quests;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Systems.Quests
{
    public class Quest
    {
        // static info
        public QuestInfo info;

        // dynamic state
        public QuestState State;

        public Quest(QuestInfo questInfo)
        {
            info = questInfo;
            State = QuestState.REQUIREMENTS_NOT_MET;
        }
    }
}