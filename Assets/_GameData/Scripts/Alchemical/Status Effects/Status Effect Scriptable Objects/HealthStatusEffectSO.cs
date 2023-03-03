using TSGameDev.Core.AI;
using UnityEngine;

namespace TSGameDev.Core.Effects
{
    [CreateAssetMenu(fileName = "New Heal Effect", menuName = "TSGameDev/Status Effects/New Heal Effect")]
    public class HealthStatusEffectSO : StatusEffectSOBase
    {
        [SerializeField] private string effectName = "Heal [Teir]";
        [SerializeField] private int healthGain = 10;
        [SerializeField] private bool isInstant = true;
        [SerializeField] private int maxTicks = 1;
        [SerializeField] private TickTime tickTime = TickTime.EveryTick;

        public override string GetEffectName() => effectName;
        public override bool GetIsInstant() => isInstant;
        
        public override int GetMaxTick()
        {
            if (isInstant)
                return 1;
            else
                return maxTicks;
        }

        public override TickTime GetTickTime() => tickTime;

        public override void RemoveStatusEffect(IEffectable _Target)
        {
            Debug.Log("This is nothing to remove for this effect");
        }

        public override void ApplyStatusEffect(IEffectable _Target)
        {
            AIStats _NewTargetStats = _Target.GetStats();
            _NewTargetStats.health += healthGain;
            _Target.SetStats(_NewTargetStats);
        }
    }
}
