using Assets.Guiding_Winds.Scripts.Systems;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Player
{

    public class PlayerRepManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private int startingRep = 0;

        private int CurrentRep;

        private RepStatus RepStatus;

        private void Awake()
        {
            CurrentRep = startingRep;
            RepStatus = RepStatus.UNKNOWN;
        }

        private void OnEnable()
        {
            GameEventsManager.Instance.PlayerEvents.OnRepGained += RepGained;
        }

        private void OnDisable()
        {
            GameEventsManager.Instance.PlayerEvents.OnRepGained -= RepGained;
        }

        private void Start()
        {
            GameEventsManager.Instance.PlayerEvents.PlayerRepChange(CurrentRep);
            GameEventsManager.Instance.PlayerEvents.PlayerRepStatusChange(RepStatus);
        }

        private void RepGained(int rep)
        {
            CurrentRep += rep;
            UpdateRepStatus();
            GameEventsManager.Instance.PlayerEvents.PlayerRepChange(CurrentRep);
        }

        private void UpdateRepStatus()
        {
            RepStatus previousStatus = RepStatus;
            if (CurrentRep >= 1000)
                RepStatus = RepStatus.EXALTED;
            else if (CurrentRep >= 500)
                RepStatus = RepStatus.TRUSTED;
            else if (CurrentRep >= 200)
                RepStatus = RepStatus.FRIENDLY;
            else if (CurrentRep >= 50)
                RepStatus = RepStatus.ACCQUAINTANCE;
            else
                RepStatus = RepStatus.UNKNOWN;

            if (previousStatus != RepStatus)
                GameEventsManager.Instance.PlayerEvents.PlayerRepStatusChange(RepStatus);
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