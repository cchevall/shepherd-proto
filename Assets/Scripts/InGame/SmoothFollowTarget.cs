using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowTarget : MonoBehaviour
{
    // camera will follow this object
    public Transform targetTransform;
    // offset between camera and target
    private Vector3 offset;
    // change this value to get desired smoothness
    private float smoothTime = .5f;

    // This value will change at the runtime depending on target movement. Initialize with zero vector.
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        offset = transform.position - targetTransform.position;
    }

    private void LateUpdate()
    {
        // update position
        Vector3 targetPosition = targetTransform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        // update rotation
        transform.LookAt(targetTransform);
    }
}
