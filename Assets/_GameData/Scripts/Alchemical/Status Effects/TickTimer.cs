using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSGameDev.Core.Effects
{
    //Class to spawn on enemy to track individual status effects
    public class TickTimer : MonoBehaviour
    {
        private int _CurrentTickCount = 0;
        private int _MaxTickCount;
        public void SetMaxTickCount(int _MaxTicks) => _MaxTickCount = _MaxTicks;
        private IStatusEffect _StatusEffect;
        public void SetStatusEffect(IStatusEffect _Effect) => _StatusEffect = _Effect;
        private IEffectable _Target;
        public void SetTarget(IEffectable _Target) => this._Target = _Target;

        public void ActivateStatusEffect(object sender, TimeTickSystem.OnTickEventArgs e)
        {
            if (_CurrentTickCount >= _MaxTickCount)
            {
                TimeTickSystem.OnTick -= ActivateStatusEffect;
                TimeTickSystem.OnTick_5 -= ActivateStatusEffect;
                _StatusEffect.RemoveStatusEffect(_Target);
                Destroy(gameObject);
            }
            else
            {
                _CurrentTickCount++;
                _StatusEffect.ApplyStatusEffect(_Target);
            }
        }

        public static void CreateTickTimer(GameObject _Parent, IEffectable _TargetEffectable, IStatusEffect _Effect)
        {
            //Create new timer object, add ticktimer class and make its parent this game object
            GameObject _NewTickTimerObj = new GameObject(_Effect.GetEffectName());
            _NewTickTimerObj.transform.parent = _Parent.transform;
            TickTimer _NewTickTimer = _NewTickTimerObj.AddComponent<TickTimer>();
            _NewTickTimer.SetStatusEffect(_Effect);
            _NewTickTimer.SetMaxTickCount(_Effect.GetMaxTick());
            _NewTickTimer.SetTarget(_TargetEffectable);
            switch (_Effect.GetTickTime())
            {
                case TickTime.EveryTick:
                    TimeTickSystem.OnTick += _NewTickTimer.ActivateStatusEffect;
                    break;
                case TickTime.Every5Tick:
                    TimeTickSystem.OnTick_5 += _NewTickTimer.ActivateStatusEffect;
                    break;
            }
        }
    }
}
