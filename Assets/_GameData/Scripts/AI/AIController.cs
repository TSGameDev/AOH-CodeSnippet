using System.Collections;
using System.Collections.Generic;
using TSGameDev.Core.AI;
using TSGameDev.Core.Effects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

namespace TSGameDev.Core.AI
{
    //Main script that for AI that holds state machine, ai data and ai brain. I.E. The main AI controller
    public class AIController : MonoBehaviour, IEffectable
    {
        #region Serialized Variables

        [SerializeField] private AIData aIData;
        [SerializeField] private AIBrain aIBrain;
        [SerializeField] private Transform returnPoint;
        [SerializeField] private Transform[] wonderPoints;

        #endregion

        #region Private Variables

        private StateMachine _StateMachine;
        private State _BeginningState;
        private Animator _Animator;
        private NavMeshAgent _NavMeshAgent;

        [SerializeField] private AIStats _InstanceData;

        #endregion

        #region Anim Delegates

        public delegate void AnimEvenetDelegate();
        private AnimEvenetDelegate AnimDelegate;

        public void SetAnimDelegate(AnimEvenetDelegate attackEvent) => this.AnimDelegate = attackEvent;
        public void TriggerAnimDelegate() => AnimDelegate?.Invoke();

        #endregion

        #region LifeCycle Functions

        private void Awake()
        {
            _InstanceData = aIData.GetAIStats();
            _StateMachine = new();
            _Animator = GetComponent<Animator>();
            _NavMeshAgent= GetComponent<NavMeshAgent>();
            _BeginningState = new IdleState(gameObject, _StateMachine, _Animator, _NavMeshAgent, _InstanceData, aIBrain, this);
        }

        private void Start()
        {
            _StateMachine.CurrentState = _BeginningState;
            _NavMeshAgent.updatePosition = false;
        }

        private void Update()
        {
            _StateMachine.CurrentState.Update();

            //creates a local vector 3 the is differene between the current ai position and the navmesh agents next position
            Vector3 worldDeltaPosition = _NavMeshAgent.nextPosition - transform.position;
            //if the magnitude of the worlddeltaposition is greater than the radius of the agent pulls the navmesh agent back to the edge of the original navmesh agent
            if (worldDeltaPosition.magnitude > _NavMeshAgent.radius)
                _NavMeshAgent.nextPosition = transform.position + 0.9f * worldDeltaPosition;


        }

        private void FixedUpdate()
        {
            _StateMachine.CurrentState.FixedUpdate();
        }

        //function for when the root motion of animations are played
        private void OnAnimatorMove()
        {
            //local variable of the currentl animators rootpositon
            Vector3 position = _Animator.rootPosition;
            //sets the Y to the Y of the agents next position
            position.y = _NavMeshAgent.nextPosition.y;
            //makes the transform of the AI to the local variable
            transform.position = position;

            //this makes sure the AI is able to walk up and down based on the navmesh agent since the navmesh can't update the position itself
        }

        #endregion

        #region Getters

        public Transform GetReturnPoint() => returnPoint;

        public Transform[] GetWonderPoints() => wonderPoints;

        #endregion

        #region Public Methods

        public void ResetInstanceStats()
        {
            _InstanceData = aIData.GetAIStats();
        }

        public void ForceDeathState() => _StateMachine.CurrentState = new DeadState(gameObject, _StateMachine, _Animator, _NavMeshAgent, _InstanceData, aIBrain, this);

        #endregion

        #region Effect Functions

        public void Effect(IStatusEffect[] _PotionStatusEffects)
        {
            Debug.Log("Potion Effect Ran");
            //spawn ticktimer for every effect
            foreach (IStatusEffect _Effect in _PotionStatusEffects)
            {
                //assign the effect function on the ticktimer to the correc tick time
                if (_Effect.GetIsInstant())
                {
                    Debug.Log("Instant Effect Activated");
                    _Effect.ApplyStatusEffect(this);
                }
                else
                {
                    TickTimer.CreateTickTimer(gameObject, this, _Effect);
                    Debug.Log("Tick Timer Created");
                }
            }
        }

        public void SetStats(AIStats _NewStats)
        { 
            _InstanceData = _NewStats;

            if (_InstanceData.health <= 0)
                ForceDeathState();
        }

        public AIStats GetStats() => _InstanceData;

        public AIData GetBaseStats() => aIData;

        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, aIData.GetAIStats().visualRange);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, aIData.GetAIStats().attackRange);
            Gizmos.color = Color.black;
            Gizmos.DrawCube(transform.position + transform.forward, new Vector3(2, 0.2f, 1));
        }
    }
}
