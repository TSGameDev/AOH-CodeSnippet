using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSGameDev.Controls.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CameraConnector cameraConnector;
        [SerializeField] private Transform cameraTranform;
        [SerializeField] private Transform playerTransform;

        [Tooltip("The minimum X world boarder for the movement of the camera")]
        [SerializeField] private float minWorldX;
        [Tooltip("The maximum X world boarder for the movement of the camera")]
        [SerializeField] private float maxWorldX;
        [Tooltip("The minimum Z world boarder for the movement of the camera")]
        [SerializeField] private float minWorldZ;
        [Tooltip("The maximum Z world boarder for the movement of the camera")]
        [SerializeField] private float maxWorldZ;

        [Tooltip("The minimum value the cams Y value can be. AkA the closest the cam can zoom in. (Needs to match the min Z but be positive)")]
        [SerializeField] private float minCamZoomY;
        [Tooltip("The maximum value the cams Y value can be. AkA the furthest the cam can zoom out. (Needs to match the max Z but be positive)")]
        [SerializeField] private float maxCamZoomY;
        [Tooltip("The minimum value the cams Z value can be. AkA the closest the cam can zoom in. (Needs to match the min Y but be negative)")]
        [SerializeField] private float minCamZoomZ;
        [Tooltip("The maximum value the cams Z value can be. AkA the furthest the cam can zoom out. (Needs to match the max Y but be negative)")]
        [SerializeField] private float maxCamZoomZ;

        private void Start()
        {
            cameraConnector.newPos = transform.position;
            cameraConnector.newRot = transform.rotation;
            cameraConnector.newZoom = cameraTranform.localPosition;
        }

        private void Update()
        {
            HandleMovementInput();
            HandleRotation();
            HandleZoom();
        }

        /// <summary>
        /// Function that handles the movement of the camera via the WASD keys
        /// </summary>
        void HandleMovementInput()
        {
            //Use fast speed if the shift key is down
            if (cameraConnector.fastMode)
                cameraConnector.currentSpeed = cameraConnector.fastSpeed;
            else
                cameraConnector.currentSpeed = cameraConnector.normalSpeed;

            //Right
            if (cameraConnector.cameraInput.x >= 0.5f)
            {
                cameraConnector.newPos += (transform.right * cameraConnector.currentSpeed);
                if (transform.position.x > maxWorldX)
                    cameraConnector.newPos.x = maxWorldX;
            }

            //Left
            else if (cameraConnector.cameraInput.x <= -0.5f)
            {
                cameraConnector.newPos += (transform.right * -cameraConnector.currentSpeed);
                if(transform.position.x < minWorldX)
                    cameraConnector.newPos.x = minWorldX;

            }

            //Forward
            if (cameraConnector.cameraInput.y >= 0.5f)
            {
                cameraConnector.newPos += (transform.forward * cameraConnector.currentSpeed);
                if (transform.position.z > maxWorldZ)
                    cameraConnector.newPos.z = maxWorldZ;
            }

            //Backward
            else if (cameraConnector.cameraInput.y <= -0.5f)
            {
                cameraConnector.newPos += (transform.forward * -cameraConnector.currentSpeed);
                if (transform.position.z < minWorldZ)
                    cameraConnector.newPos.z = minWorldZ;
            }

            if (cameraConnector.lockCamera)
            {
                Vector3 playerPos = playerTransform.position;
                playerPos.y = gameObject.transform.position.y;
                cameraConnector.newPos = playerPos;
            }

            transform.position = Vector3.Lerp(transform.position, cameraConnector.newPos, Time.deltaTime * cameraConnector.movementLerpTime);
        }

        /// <summary>
        /// Function that handles the rotation of the camera via the QE keys
        /// </summary>
        void HandleRotation()
        {
            //Rotate Clockwise
            if (cameraConnector.rotateCameraRight)
            {
                cameraConnector.newRot *= Quaternion.Euler(Vector3.up * cameraConnector.rotationTickAmount);
            }
            //Rotate anti-Clockwise
            else if (cameraConnector.rotateCameraLeft)
            {
                cameraConnector.newRot *= Quaternion.Euler(Vector3.up * -cameraConnector.rotationTickAmount);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, cameraConnector.newRot, Time.deltaTime * cameraConnector.movementLerpTime);
        }

        /// <summary>
        /// Function that handles the zooming of the camera via the mouse scroll wheel
        /// </summary>
        void HandleZoom()
        {
            if (cameraConnector.zoomCameraIn)
            {
                cameraConnector.newZoom -= cameraConnector.zoomTickAmount;
            }
            else if (cameraConnector.zoomCameraOut)
            {
                cameraConnector.newZoom += cameraConnector.zoomTickAmount;
            }

            if (cameraConnector.newZoom.y <= minCamZoomY || cameraConnector.newZoom.z >= minCamZoomZ)
            {
                cameraConnector.newZoom = new Vector3(0, minCamZoomY, minCamZoomZ);
            }
            else if (cameraConnector.newZoom.y > maxCamZoomY || cameraConnector.newZoom.z < maxCamZoomZ)
            {
                cameraConnector.newZoom = new Vector3(0, maxCamZoomY, maxCamZoomZ);
            }

            cameraTranform.localPosition = Vector3.Lerp(cameraTranform.localPosition, cameraConnector.newZoom, Time.deltaTime * cameraConnector.movementLerpTime);
        }
    }
}
