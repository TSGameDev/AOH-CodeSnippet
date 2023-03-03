using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TSGameDev.Core.AI
{
    public class ReturnState : State
    {
        public ReturnState(GameObject _Entity, StateMachine _StateMachine, Animator _Anim, NavMeshAgent _Agent, AIStats _AIStats, AIBrain _AIBrain, AIController _AIController) 
            : base(_Entity, _StateMachine, _Anim, _Agent, _AIStats, _AIBrain, _AIController)
        {
        }

        public override void Enter()
        {
            Debug.Log("Return State");
            _EntityAnimator.SetFloat(_AIBrain.speedHash, ANIM_WALK_SPEED);
            _EntityNavMeshAgent.ResetPath();

            if(_AIBrain.GetWonder())
                _StateMachine.CurrentState = new WounderState(_Entity, _StateMachine, _EntityAnimator, _EntityNavMeshAgent, _AIStats, _AIBrain, _AIController);
            else
                _EntityNavMeshAgent.SetDestination(_AIController.GetReturnPoint().position);
            
            _AIController.ResetInstanceStats();
        }

        public override void Update()
        {
            float _RemainingDis = DistanceCheck(_EntityNavMeshAgent.destination);
            if (_RemainingDis <= 0.5f)
                _StateMachine.CurrentState = new IdleState(_Entity, _StateMachine, _EntityAnimator, _EntityNavMeshAgent, _AIStats, _AIBrain, _AIController);
        }
    }
}
