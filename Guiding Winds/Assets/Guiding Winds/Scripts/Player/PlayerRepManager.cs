using Assets.Guiding_Winds.Scripts.Systems;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Player
{

    public class PlayerRepManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private int startingRep = 0;

        private int currentRep;

        public RepStatus RepStatus;

        private void Awake()
        {
            currentRep = startingRep;
            RepStatus = RepStatus.UNKNOWN;
        }

        private void OnEnable()
        {
            GameEventsManager.Instance.playerEvents.OnRepGained += RepGained;
        }

        private void OnDisable()
        {
            GameEventsManager.Instance.playerEvents.OnRepGained -= RepGained;
        }

        private void Start()
        {
            GameEventsManager.Instance.playerEvents.PlayerRepChange(currentRep);
        }

        private void RepGained(int rep)
        {
            currentRep += rep;
            UpdateRepStatus();
            GameEventsManager.Instance.playerEvents.PlayerRepChange(currentRep);
        }

        private void UpdateRepStatus()
        {
            if (currentRep >= 1000)
                RepStatus = RepStatus.EXALTED;
            else if (currentRep >= 500)
                RepStatus = RepStatus.TRUSTED;
            else if (currentRep >= 200)
                RepStatus = RepStatus.FRIENDLY;
            else if (currentRep >= 50)
                RepStatus = RepStatus.ACCQUAINTANCE;
            else
                RepStatus = RepStatus.UNKNOWN;
        }
    }

    public enum RepStatus
    {
        UNKNOWN, // 0
        ACCQUAINTANCE, // 1
        FRIENDLY, // 2
        TRUSTED, // 3
        EXALTED // 4
    }
}