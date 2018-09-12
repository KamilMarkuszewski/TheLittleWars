using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public int Drag;
    public Transform target;
    public Vector3 Offset = new Vector3(0f, 7.5f, 0f);


    private void LateUpdate()
    {
        if (target != null)
        {
            ApplyPosition(target, Offset, Drag);
        }
    }

    public void ApplyPosition(Transform target, Vector3 offset, int drag)
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * Drag);
    }
}
