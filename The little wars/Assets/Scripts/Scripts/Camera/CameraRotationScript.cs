using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationScript : MonoBehaviour
{
    public int AngularDrag;
    public Transform Target;

    private void LateUpdate()
    {
        if (Target != null)
        {
            ApplyRotation(Target, AngularDrag);
        }
    }

    public void ApplyRotation(Transform target, int angularDrag)
    {
        var bodyUp = transform.up;
        var targetUp = new Vector3(target.up.x, target.up.y, 0);
        var targetRot = Quaternion.FromToRotation(bodyUp, targetUp) * transform.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * AngularDrag);
    }
}
