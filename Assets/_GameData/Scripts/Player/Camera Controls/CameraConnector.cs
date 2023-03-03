using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSGameDev.Controls.Camera
{
    [CreateAssetMenu(fileName = "New Camera Connector", menuName = "TSGameDev/Camera Controls/New Camera Connector")]
    public class CameraConnector : ScriptableObject
    {
        #region Camera Movement Variables

        public float normalSpeed;
        public float fastSpeed;
        public float currentSpeed;
        public float movementLerpTime;
        public float rotationTickAmount;
        public Vector3 zoomTickAmount;

        public Vector3 newPos;
        public Quaternion newRot;
        public Vector3 newZoom;

        #endregion

        #region Camera Inputs

        public Vector2 cameraInput;

        public bool fastMode;
        public bool rotateCameraRight;
        public bool rotateCameraLeft;
        public bool zoomCameraIn;
        public bool zoomCameraOut;
        public bool lockCamera;

        #endregion
    }
}

