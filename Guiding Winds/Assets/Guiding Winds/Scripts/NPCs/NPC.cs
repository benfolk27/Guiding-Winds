using UnityEngine;
using UnityEngine.AI;

namespace Assets.Guiding_Winds.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class NPC : MonoBehaviour
    {

        [HideInInspector]
        public NavMeshAgent Agent;

        [HideInInspector]
        public Animator Animator;

        public float CurrentSpeed {  get { return Agent.velocity.magnitude;  } }

        public void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
        }


        //void Start()
        //{

        //}

        
        //void Update()
        //{

        //}
    }

}