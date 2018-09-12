using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class CameraHelper
    {
        private static CameraFollowScript _cameraFollowScript;
        private static CameraFollowScript cameraFollowScript
        {
            get
            {
                if (_cameraFollowScript == null)
                {
                    _cameraFollowScript = Camera.main.GetComponent<CameraFollowScript>();
                }
                return _cameraFollowScript;
            }
        }

        private static CameraRotationScript _cameraRotationScript;
        private static CameraRotationScript cameraRotationScript
        {
            get
            {
                if (_cameraRotationScript == null)
                {
                    _cameraRotationScript = Camera.main.GetComponent<CameraRotationScript>();
                }
                return _cameraRotationScript;
            }
        }

        public static void SetCameraFollowAndRotationTarget(Transform target)
        {
            SetCameraFollowTarget(target);
            SetCameraRotationTarget(target);
        }

        public static void SetCameraFollowTarget(Transform target)
        {
            cameraFollowScript.target = target;
        }

        public static void SetCameraRotationTarget(Transform target)
        {
            cameraRotationScript.target = target;
        }
    }
}
