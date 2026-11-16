using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.NPCs
{
    public class NPCWander : NPCComponent
    {
        public Area Area;

        private void Start()
        {
            if (Random.Range(0f, 100.0f) > 50f)
            {
                ChangeState(ESTate.Wandering);
            }
            else
            {
                ChangeState(ESTate.Waiting);
            }
        }
         
        private void Update()
        {
            if (state == ESTate.Waiting)
            {
                waitTime -= Time.deltaTime;

                if (waitTime < 0f)
                {
                    ChangeState(ESTate.Wandering);
                }
            }
            else if (state == ESTate.Wandering)
            {
                wanderingTime -= Time.deltaTime;
                if (HasArrived() || wanderingTime < 0f)
                {
                    ChangeState(ESTate.Waiting);
                }
            }
        }

        enum ESTate
        {
            Wandering,
            Waiting,
            Stopping
        }

        [Header("Debugging")]
        [SerializeField]
        ESTate state = ESTate.Wandering;

        [SerializeField]
        float maxWaitTime = 10f;

        [SerializeField]
        float minWaitTime = 1f;

        [SerializeField]
        private float waitTime = 0f;

        [Space(15f)]

        [SerializeField]
        float maxWanderingTime = 20f;

        [SerializeField]
        float minWanderingTime = 5f;

        [SerializeField]
        private float wanderingTime = 0f;


        void ChangeState(ESTate newState, Vector3? destination = null)
        {
            state = newState;

            if (state == ESTate.Wandering)
            {
                npc.Agent.isStopped = false;
                if (destination != null)
                {
                    npc.Agent.SetDestination((Vector3)destination);
                }
                else { SetRandomDestination(); }

                wanderingTime = Random.Range(minWanderingTime, maxWanderingTime);
            }
            else if (state == ESTate.Waiting)
            {
                waitTime = Random.Range(minWaitTime, maxWaitTime);
                npc.Agent.isStopped = true;
            }
            else
            {
                npc.Agent.isStopped = true;
            }
        }

        bool HasArrived()
        {
            return npc.Agent.remainingDistance <= npc.Agent.stoppingDistance;
        }

        private void SetRandomDestination()
        {
            npc.Agent.SetDestination(Area.GetRandomPoint());
        }
    }
}