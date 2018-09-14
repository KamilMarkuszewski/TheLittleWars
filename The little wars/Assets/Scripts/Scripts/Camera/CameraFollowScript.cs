using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Scripts.Camera
{
    public class CameraFollowScript : MonoBehaviour
    {
        public int Drag;
        public Transform Target;
        public Vector3 Offset = new Vector3(0f, 7.5f, 0f);


        private void LateUpdate()
        {
            if (Target != null)
            {
                ApplyPosition(Target, Offset, Drag);
            }
        }

        public void ApplyPosition(Transform target, Vector3 offset, int drag)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * Drag);
        }
    }
}
