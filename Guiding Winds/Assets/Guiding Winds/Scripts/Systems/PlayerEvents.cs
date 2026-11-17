using Assets.Guiding_Winds.Scripts.Player;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Guiding_Winds.Scripts.Systems
{
    public class PlayerEvents
    {
        public event Action<int> OnRepGained;
        public void RepGained(int rep)
        {
            OnRepGained?.Invoke(rep);
        }

        public event Action<int> OnPlayerRepChange;
        public void PlayerRepChange(int rep)
        {
            OnPlayerRepChange?.Invoke(rep);
        }

        public event Action<RepStatus> OnRepStatusChange;
        public void PlayerRepStatusChange(RepStatus reps)
        {
            OnRepStatusChange?.Invoke(reps);
        }
    }
}