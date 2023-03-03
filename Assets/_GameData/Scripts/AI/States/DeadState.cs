using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TSGameDev.Core.AI
{
    public class DeadState : State
    {
        public DeadState(GameObject _Entity, StateMachine _StateMachine, Animator _Anim, NavMeshAgent _Agent, AIStats _AIStats, AIBrain _AIBrain, AIController _AIController) : base(_Entity, _StateMachine, _Anim, _Agent, _AIStats, _AIBrain, _AIController)
        {
        }

        public override void Enter()
        {
            _EntityAnimator.SetTrigger(_AIBrain.deadHash);
            _EntityNavMeshAgent.enabled = false;
            _AIController.enabled = false;
        }
    }
}
