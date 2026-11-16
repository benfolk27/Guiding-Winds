using UnityEngine;

namespace Assets.Guiding_Winds.Scripts
{
    public class NPCAnimator : NPCComponent
    {
        private void Update()
        {
            npc.Animator.SetFloat("Speed", npc.CurrentSpeed);
        }
    }
}