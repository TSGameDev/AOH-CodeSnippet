using UnityEngine;
using UnityEngine.AI;
using TSGameDev.Controls.PlayerStates;
using System.Collections.Generic;

namespace TSGameDev.Controls.MainPlayer
{
    [RequireComponent(typeof(InputManager))]
    public class Player : MonoBehaviour
    {
        #region State

        public PlayerState state;

        #endregion

        #region Movement Variables

        NavMeshAgent agent;
        Animator anim;

        public float rotationSpeed = 1f;
        public bool isRunning;

        #endregion

        #region Anim Keys

        public readonly int animSpeed = Animator.StringToHash("Speed");

        #endregion

        #region Interact Variables

        private IInteractable previousInterable;
        private Vector3 interactMarker;

        #endregion

        #region Serialized Variables

        [SerializeField] float interactRadius;
        [SerializeField] float interactCancelDistance;

        #endregion

        #region Lifecycle Functions

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            agent.updatePosition = false;
            state = new PlayerStateIdle(this);
        }

        private void Update()
        {
            state.Update();

            //creates a local vector 3 the is differene between the current ai position and the navmesh agents next position
            Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
            //if the magnitude of the worlddeltaposition is greater than the radius of the agent pulls the navmesh agent back to the edge of the original navmesh agent
            if (worldDeltaPosition.magnitude > agent.radius)
                agent.nextPosition = transform.position + 0.9f * worldDeltaPosition;

            PlayerMarkerDistanceCheck();
        }

        //function for when the root motion of animations are played
        private void OnAnimatorMove()
        {
            //local variable of the currentl animators rootpositon
            Vector3 position = anim.rootPosition;
            //sets the Y to the Y of the agents next position
            position.y = agent.nextPosition.y;
            //makes the transform of the AI to the local variable
            transform.position = position;

            //this makes sure the AI is able to walk up and down based on the navmesh agent since the navmesh can't update the position itself
        }

        #endregion

        #region Public Functions

        public void RadialInteract()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRadius);
            foreach(Collider obj in colliders)
            {
                if(obj.TryGetComponent<IInteractable>(out var interactableObj))
                {
                    if (interactableObj.IsToggleable())
                        PlaceInteractMarker();
                    interactableObj.OnInteract();
                    previousInterable = interactableObj;
                    return;
                }
            }
        }

        #endregion

        #region Private Functions

        private void PlaceInteractMarker()
        {
            interactMarker = gameObject.transform.position;
        }

        private void PlayerMarkerDistanceCheck()
        {
            if (previousInterable == null || interactMarker == Vector3.zero)
                return;

            if(Vector3.Distance(transform.position, interactMarker) >= interactCancelDistance)
            {
                previousInterable.Cancel();
                interactMarker = Vector3.zero;
                previousInterable = null;
            }
        }

        #endregion

    }
}
