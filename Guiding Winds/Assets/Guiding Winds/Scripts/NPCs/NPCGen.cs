using System.Collections;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.NPCs
{
    public class NPCGenerator : MonoBehaviour
    {

        [SerializeField]
        NPC NPCPrefab;

        [SerializeField]
        Area Area;

        [SerializeField]
        int Count = 5;

        private void Start()
        {
            for (int i = 0; i < Count; i++)
            {
                Vector3 position = Area.GetRandomPoint();

                Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                NPC npc = Instantiate(NPCPrefab, position, rotation);

                npc.GetComponent<NPCWander>().Area = Area;
            }
        }
    }
}