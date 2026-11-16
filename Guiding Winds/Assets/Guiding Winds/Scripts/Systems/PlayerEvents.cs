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
        public void PlayerRepChange(int experience)
        {
            OnPlayerRepChange?.Invoke(experience);
        }
    }
}