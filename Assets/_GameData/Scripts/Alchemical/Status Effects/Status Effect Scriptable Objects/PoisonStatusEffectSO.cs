using TSGameDev.Core.AI;
using UnityEngine;

namespace TSGameDev.Core.Effects
{
    [CreateAssetMenu(fileName = "New Poison Effect", menuName = "TSGameDev/Status Effects/New Poison Effect")]
    public class PoisonStatusEffectSO : StatusEffectSOBase
    {
        [SerializeField] private string effectName = "Poison [Tier]";
        [SerializeField] private bool isInstant = false;
        [SerializeField] private int maxTicks = 10;
        [SerializeField] private TickTime tickTime = TickTime.EveryTick;
        [Tooltip("The damage deal every tick, if the poison is set to instant it does this damange only once")]
        [SerializeField] private int damagePerTick = 1;

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
            _NewTargetStats.health -= damagePerTick;
            _Target.SetStats(_NewTargetStats);
        }
    }
}
