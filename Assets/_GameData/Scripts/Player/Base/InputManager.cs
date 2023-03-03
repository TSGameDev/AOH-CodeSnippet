using UnityEngine;
using TSGameDev.Controls.Camera;
using TSGameDev.Controls.MainPlayer;
using TSGameDev.UI.Controls;
using TSGameDev.Inventories.Actions;
using UnityEngine.InputSystem;
using System;

namespace TSGameDev.Controls
{
    public class InputManager : MonoBehaviour
    {
        #region Player Variables

        private Player _Player;
        private PlayerControls _PlayerControls;
        private ActionStore _PlayerActionStore;

        #endregion

        #region Camera Variables

        [SerializeField] CameraConnector cameraConnector;
        [SerializeField] PlayerConnector playerConnector;
        [SerializeField] UIConnector uiConnector;

        #endregion


        [SerializeField] private LayerMask mouseHitPointLayer;
        [SerializeField] private LayerMask indicatorLayer;
        [SerializeField] private Canvas indicatorImage;

        [HideInInspector]
        public Vector3 MouseRayPoint;

        private void OnEnable()
        {
            _Player = GetComponent<Player>();
            _PlayerActionStore = GetComponent<ActionStore>();

            if (_PlayerControls == null)
                _PlayerControls = new();

            _PlayerControls.Enable();
            _PlayerControls.Game.Enable();

            #region Movement Controls

            _PlayerControls.Game.MouseRightClick.performed += ctx => _Player.state.MoveTo();

            _PlayerControls.Game.ShiftHold.performed += ctx => _Player.isRunning = !_Player.isRunning;
            _PlayerControls.Game.ShiftHold.performed += ctx => cameraConnector.fastMode = !cameraConnector.fastMode;

            #endregion

            #region Camera Controls

            _PlayerControls.Game.CameraMovement.performed += ctx => cameraConnector.cameraInput = ctx.ReadValue<Vector2>();
            _PlayerControls.Game.CameraMovement.canceled += ctx => cameraConnector.cameraInput = new Vector2();

            _PlayerControls.Game.CameraRotation.performed += ctx =>
            {
                float rotationValue = ctx.ReadValue<float>();
                if (rotationValue >= 0.5f)
                {
                    cameraConnector.rotateCameraRight = true;
                    cameraConnector.rotateCameraLeft = false;
                }
                else if (rotationValue <= -0.5f)
                {
                    cameraConnector.rotateCameraLeft = true;
                    cameraConnector.rotateCameraRight = false;
                }
            };
            _PlayerControls.Game.CameraRotation.canceled += ctx =>
            {
                cameraConnector.rotateCameraLeft = false;
                cameraConnector.rotateCameraRight = false;
            };

            _PlayerControls.Game.CameraZoom.performed += ctx =>
            {
                float zoomValue = ctx.ReadValue<float>();
                if (zoomValue > 1)
                {
                    cameraConnector.zoomCameraIn = true;
                    cameraConnector.zoomCameraOut = false;
                }

                else if (zoomValue < -1)
                {
                    cameraConnector.zoomCameraIn = false;
                    cameraConnector.zoomCameraOut = true;
                }
            };
            _PlayerControls.Game.CameraZoom.canceled += ctx =>
            {
                cameraConnector.zoomCameraIn = false;
                cameraConnector.zoomCameraOut = false;
            };

            _PlayerControls.Game.CameraCenter.performed += ctx => cameraConnector.lockCamera = !cameraConnector.lockCamera;

            #endregion

            #region Interface Controls

            _PlayerControls.Game.Inventory.performed += ctx => uiConnector.InventoryTween();
            _PlayerControls.Game.Equipment.performed += ctx => uiConnector.EquipmentTween();

            #endregion

            #region Gameplay Controls

            _PlayerControls.Game.Interaction.performed += ctx => _Player.RadialInteract();
            _PlayerControls.Game.MouseDelta.performed += ctx => UpdateMousePointHit();
            _PlayerControls.Game.ActionItems.performed += ctx => _PlayerActionStore.ActivateThrow(ctx.ReadValue<float>());

            #endregion

        }

        private Action<InputAction.CallbackContext> handler;

        public void SetThrowAction(int _ActionItemIndex)
        {
            _PlayerControls.Game.ThrowItem.performed -= handler;
            handler = (InputAction.CallbackContext ctx) => SetThrowPerform(ctx, _ActionItemIndex);
            _PlayerControls.Game.ThrowItem.performed += handler;
            ToggleIndicator(true);
        }

        private void SetThrowPerform(int _ActionItemIndex)
        {
            //Perform Item Use
            _PlayerActionStore.Use(_ActionItemIndex, gameObject, indicatorImage.transform.position);
            //Reset indicator and throw action
            ToggleIndicator(false);
            _PlayerControls.Game.ThrowItem.performed -= handler;
        }

        private void SetThrowPerform(InputAction.CallbackContext ctx, int _ActionItemIndex)
        {
            SetThrowPerform(_ActionItemIndex);
        }

        private void UpdateMousePointHit()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mouseHitPointLayer))
            {
                MouseRayPoint = hit.point;
            }
        }

        public void ToggleIndicator(bool _IndicatorActive)
        {
            indicatorImage.gameObject.SetActive(_IndicatorActive);
        }

        private void Update()
        {
            PositionIndicator();
        }

        public void PositionIndicator()
        {
            Vector3 _MousePosCache = MouseRayPoint;

            Vector3 HitPosDir = (_MousePosCache - transform.position).normalized;
            float distance = Vector3.Distance(_MousePosCache, transform.position);
            distance = Mathf.Min(distance, _PlayerActionStore.GetThrowDis());
            var newHitPos = transform.position + HitPosDir * distance;

            indicatorImage.transform.position = newHitPos + new Vector3(0, 0.1f, 0);
        }

        private void OnDisable()
        {
            _PlayerControls.Disable();
        }
    }
}
