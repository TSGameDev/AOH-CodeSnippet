using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TSGameDev.Core.AI
{
    public class WounderState : State
    {
        private Transform[] wonderPoints;

        public WounderState(AIController _AIController) : base(_AIController)
        {
        }

        public override void Enter()
        {
            Debug.Log("Wonder State");
            wonderPoints = _AIController.GetWonderPoints();
            _EntityAnimator.SetFloat(_AIBrain.speedHash, ANIM_WALK_SPEED);
        }

        public override void Update()
        {
            float _DisBetweenEntityWonderPoint = DistanceCheck(_EntityNavMeshAgent.destination);
            if (_DisBetweenEntityWonderPoint < NAV_AGENT_DESTINATION_OFFSET)
                SetNewWonderPoint();
        }

        public override void FixedUpdate()
        {
            CheckForPlayer();
        }

        private void SetNewWonderPoint()
        {
            if(_EntityNavMeshAgent.hasPath)
                _EntityNavMeshAgent.ResetPath();
            
            int _RandomWonderPoint = Random.Range(0, wonderPoints.Length);
            Transform _SelectedWonderPoint = wonderPoints[_RandomWonderPoint];
            _EntityNavMeshAgent.SetDestination(_SelectedWonderPoint.position);
        }
    }
}
