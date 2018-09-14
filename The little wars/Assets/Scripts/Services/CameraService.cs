using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Scripts.Camera;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class CameraService : IService
    {
        private CameraFollowScript _cameraFollowScript;
        private CameraFollowScript CameraFollowScript
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

        private CameraRotationScript _cameraRotationScript;
        private CameraRotationScript CameraRotationScript
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

        public void SetCameraFollowAndRotationTarget(Transform target)
        {
            SetCameraFollowTarget(target);
            SetCameraRotationTarget(target);
        }

        public void SetCameraFollowTarget(Transform target)
        {
            CameraFollowScript.Target = target;
        }

        public void SetCameraRotationTarget(Transform target)
        {
            CameraRotationScript.Target = target;
        }


        #region IService

        public void Initialize()
        {
            Status = ServiceStatus.Initializing;

            Status = ServiceStatus.Ready;
        }

        public ServiceStatus Status
        {
            get; private set;
        }

        #endregion
    }
}
