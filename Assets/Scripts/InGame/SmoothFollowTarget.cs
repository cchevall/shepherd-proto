using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowTarget : MonoBehaviour
{
    // camera will follow this object
    [SerializeField] Transform targetTransform;
    // change this value to get desired smoothness
    [SerializeField] float smoothTime = .5f;

    // offset between camera and target
    private Vector3 offset;

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
