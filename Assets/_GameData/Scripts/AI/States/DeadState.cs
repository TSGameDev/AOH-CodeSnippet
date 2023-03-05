using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TSGameDev.Core.AI
{
    public class DeadState : State
    {
        public DeadState(AIController _AIController) : base(_AIController)
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
