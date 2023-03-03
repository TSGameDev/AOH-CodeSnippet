using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace TSGameDev.Core.AI
{
    public class IdleState : State
    {
        public IdleState(GameObject _Entity, StateMachine _StateMachine, Animator _Anim, NavMeshAgent _Agent, AIStats _AIStats, AIBrain _AIBrain, AIController _AIController) 
            : base(_Entity, _StateMachine, _Anim, _Agent, _AIStats, _AIBrain, _AIController)
        {
        }

        public override void Enter()
        {
            if (_AIBrain.GetWonder())
            {
                _StateMachine.CurrentState = new WounderState(_Entity, _StateMachine, _EntityAnimator, _EntityNavMeshAgent, _AIStats, _AIBrain, _AIController);
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
