using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TSGameDev.Core.AI
{
    public abstract class State
    {
        protected GameObject _Entity;
        protected StateMachine _StateMachine;
        protected Animator _EntityAnimator;
        protected NavMeshAgent _EntityNavMeshAgent;
        protected AIStats _AIStats;
        protected AIBrain _AIBrain;
        protected AIController _AIController;

        protected float NAV_AGENT_DESTINATION_OFFSET = 0.5f;
        protected float ANIM_IDLE_SPEED = 0f;
        protected float ANIM_WALK_SPEED = 1f;
        protected float ANIM_RUN_SPEED = 2f;
        protected int PLAYER_COLLISION_LAYER = LayerMask.GetMask("Player");
        protected int ENTITY_COLLISION_LAYER = LayerMask.GetMask("Entity");

        private float COLLISION_COLLIDER_OFFSET = 0.25f;

        public State(AIController _AIController)
        {
            _Entity = _AIController.GetEntity();
            _StateMachine = _AIController.GetStateMachine();
            _EntityAnimator = _AIController.GetAnimator();
            _EntityNavMeshAgent = _AIController.GetNavMeshAgent();
            _AIStats = _AIController.GetInstanceData();
            _AIBrain = _AIController.GetAIBrain();
            this._AIController = _AIController;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Exit() { }

        protected float DistanceCheck(Vector3 _TargetPoint) => Vector3.Distance(_Entity.transform.position, _TargetPoint);
        protected float DistanceCheckWithCollisionOffset(Vector3 _TargetPoint) => Vector3.Distance(_Entity.transform.position, _TargetPoint) - COLLISION_COLLIDER_OFFSET;
        protected void CheckForPlayer()
        {
            Collider[] _PlayersNearEntity = Physics.OverlapSphere(_Entity.transform.position, _AIStats.visualRange, PLAYER_COLLISION_LAYER);
            if (_PlayersNearEntity != null)
            {
                float _MinDis = Mathf.Infinity;
                GameObject _ChaseTarget = null;
                foreach (Collider _Player in _PlayersNearEntity)
                {
                    float _DisBetweenPlayerEntity = Vector3.Distance(_Entity.transform.position, _Player.transform.position);
                    if (_DisBetweenPlayerEntity < _MinDis)
                    {
                        _MinDis = _DisBetweenPlayerEntity;
                        _ChaseTarget = _Player.gameObject;
                    }
                }
                if (_ChaseTarget != null)
                    _StateMachine.CurrentState = new ChaseState(_AIController, _ChaseTarget);
            }
        }
    }
}
