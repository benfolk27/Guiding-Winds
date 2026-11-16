using System.Collections;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts
{
    public class NPCComponent : MonoBehaviour
    {

        protected NPC npc;

        protected virtual void Awake()
        {
            npc = GetComponentInParent<NPC>();
        }
    }
}