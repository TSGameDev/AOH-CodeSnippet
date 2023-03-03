
using TSGameDev.Core.AI;
using TSGameDev.Core.Effects;
using UnityEngine;

namespace TSGameDev
{
    public class StatusEffectSOBase : ScriptableObject, IStatusEffect
    {
        public virtual string GetEffectName()
        {
            throw new System.NotImplementedException();
        }

        public virtual bool GetIsInstant()
        {
            throw new System.NotImplementedException();
        }

        public virtual int GetMaxTick()
        {
            throw new System.NotImplementedException();
        }

        public virtual TickTime GetTickTime()
        {
            throw new System.NotImplementedException();
        }

        public virtual void RemoveStatusEffect(IEffectable _Target)
        {
            throw new System.NotImplementedException();
        }

        public virtual void ApplyStatusEffect(IEffectable _Target)
        {
            throw new System.NotImplementedException();
        }
    }
}
