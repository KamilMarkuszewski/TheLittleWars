using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public int AngularDrag;
    public int Drag;
    public Transform target;
    public Vector3 offset = new Vector3(0f, 7.5f, 0f);


    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * Drag);

            var bodyUp = transform.up;
            var targetUp = new Vector3(target.up.x, target.up.y, 0);
            var targetRot = Quaternion.FromToRotation(bodyUp, targetUp) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * AngularDrag);
        }
    }
}
