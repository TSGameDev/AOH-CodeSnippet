using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TSGameDev.Core.AI
{
    public class ChaseState : State
    {
        private GameObject _ChaseTarget;
        public ChaseState(GameObject _Entity, StateMachine _StateMachine, Animator _Anim, NavMeshAgent _Agent, AIStats _AIStats, AIBrain _AIBrain, AIController _AIController, GameObject _ChaseTarget) : base(_Entity, _StateMachine, _Anim, _Agent, _AIStats, _AIBrain, _AIController)
        {
            this._ChaseTarget = _ChaseTarget;
        }

        public override void Enter() 
        {
            Debug.Log($"Now Chaseing {_ChaseTarget.name}");
        }

        public override void Update()
        {
            ChaseTarget();
        }

        public void ChaseTarget()
        {
            //Calculate distance between target and entity
            float _DisToTarget = DistanceCheckWithCollisionOffset(_ChaseTarget.transform.position);
            //If the distance between target and entity is less than attack range
            if (_DisToTarget <= _AIStats.attackRange)
            {
                //Transition to attack state
                _StateMachine.CurrentState = new AttackState(_Entity, _StateMachine, _EntityAnimator, _EntityNavMeshAgent, _AIStats, _AIBrain, _AIController, _ChaseTarget);
                Debug.Log("Changing state to attack state");
            }
            //If the distance between the target and entity is less than the visual range
            else if(_DisToTarget > _AIStats.visualRange)
            {
                //Transition to return state
                _StateMachine.CurrentState = new ReturnState(_Entity, _StateMachine, _EntityAnimator, _EntityNavMeshAgent, _AIStats, _AIBrain, _AIController);
                Debug.Log("Changing state to return state");
            }
            //If the distance between target and entity is greater than visual range
            else
            {
                //Run towards target
                _EntityNavMeshAgent.SetDestination(_ChaseTarget.transform.position);
                _EntityAnimator.SetFloat(_AIBrain.speedHash, ANIM_RUN_SPEED);
            }
        }

    }
}
