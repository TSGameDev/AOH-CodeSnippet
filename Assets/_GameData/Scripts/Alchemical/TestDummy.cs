using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TSGameDev.Core.AI;
using TSGameDev.Inventories;
using UnityEngine;

namespace TSGameDev.Core.Effects
{
    public class TestDummy : MonoBehaviour, IEffectable
    {
        [SerializeField] private AIData baseStats;
        [SerializeField] private AIStats _AiStatsInstance = new();

        private void Awake()
        {
            _AiStatsInstance = baseStats.GetAIStats();
        }

        public void Effect(IStatusEffect[] _PotionStatusEffects)
        {
            UnityEngine.Debug.Log("Target Dummy Effect Ran");
            //spawn ticktimer for every effect
            foreach(IStatusEffect _Effect in _PotionStatusEffects)
            {
                //assign the effect function on the ticktimer to the correc tick time
                if (_Effect.GetIsInstant())
                {
                    UnityEngine.Debug.Log("Instant Effect Activated");
                    _Effect.ApplyStatusEffect(this);
                }
                else
                    TickTimer.CreateTickTimer(gameObject, this, _Effect);
            }
        }

        public void SetStats(AIStats _NewStats) => _AiStatsInstance = _NewStats;

        public AIStats GetStats() => _AiStatsInstance;

        public AIData GetBaseStats() => baseStats;
    }
}
