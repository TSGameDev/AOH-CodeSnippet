using UnityEngine;
using TSGameDev.Inventories;
using TSGameDev.Core.AI;

namespace TSGameDev.Core.Effects
{
    public interface IEffectable
    {
        public void Effect(IStatusEffect[] potionStatusEffects);
        public void SetStats(AIStats _NewStats);
        public AIStats GetStats();
        public AIData GetBaseStats();
    }
}

