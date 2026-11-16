using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Guiding_Winds.Scripts.NPCs
{
    public class Area : MonoBehaviour
    {

        public float Radius = 20f;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }

        public Vector3 GetRandomPoint()
        {
            Vector3 randomDirection = Random.insideUnitSphere * Radius;
            randomDirection.y = 0f;

            Vector3 randomPoint = transform.position + randomDirection;

            Vector3 finalPosition = transform.position;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, 1))
            {
                finalPosition = hit.position;
            }

            return finalPosition;
        }
    }
}