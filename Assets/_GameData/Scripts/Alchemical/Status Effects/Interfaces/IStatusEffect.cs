using System;
using TSGameDev.Core.AI;
using UnityEngine;

namespace TSGameDev.Core.Effects
{
    [Serializable]
    public enum TickTime
    {
        EveryTick,
        Every5Tick
    }

    public interface IStatusEffect
    {
        public TickTime GetTickTime(); 
        public int GetMaxTick();
        public bool GetIsInstant();
        public string GetEffectName();

        public void ApplyStatusEffect(IEffectable _Target);
        public void RemoveStatusEffect(IEffectable _Target);
    }
}
