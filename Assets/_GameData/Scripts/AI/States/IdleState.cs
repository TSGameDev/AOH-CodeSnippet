using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace TSGameDev.Core.AI
{
    public class IdleState : State
    {
        public IdleState(AIController _AIController) : base(_AIController)
        {
        }

        public override void Enter()
        {
            if (_AIBrain.GetWonder())
            {
                _StateMachine.CurrentState = new WounderState(_AIController);
                return;
            }
            Debug.Log("Idle State");
            _EntityAnimator.SetFloat(_AIBrain.speedHash, ANIM_IDLE_SPEED);
        }

        public override void FixedUpdate()
        {
            CheckForPlayer();
        }
    }
}
