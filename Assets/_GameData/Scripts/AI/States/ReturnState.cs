using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TSGameDev.Core.AI
{
    public class ReturnState : State
    {
        public ReturnState(AIController _AIController) : base(_AIController)
        {
        }

        public override void Enter()
        {
            Debug.Log("Return State");
            _EntityAnimator.SetFloat(_AIBrain.speedHash, ANIM_WALK_SPEED);
            _EntityNavMeshAgent.ResetPath();

            if(_AIBrain.GetWonder())
                _StateMachine.CurrentState = new WounderState(_AIController);
            else
                _EntityNavMeshAgent.SetDestination(_AIController.GetReturnPoint().position);
            
            _AIController.ResetInstanceStats();
        }

        public override void Update()
        {
            float _RemainingDis = DistanceCheck(_EntityNavMeshAgent.destination);
            if (_RemainingDis <= 0.5f)
                _StateMachine.CurrentState = new IdleState(_AIController);
        }
    }
}
