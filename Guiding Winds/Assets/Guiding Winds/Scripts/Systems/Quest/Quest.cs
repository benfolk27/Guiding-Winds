using Assets.Guiding_Winds.Scripts.Systems.Quest;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Systems.Quest
{
    public class Quest
    {
        // static info
        public QuestInfo info;

        public Quest(QuestInfo questInfo)
        {
            info = questInfo;
        }
    }
}