using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowTarget : MonoBehaviour
{
    // camera will follow this object
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform aimTransform;
    // change this value to get desired smoothness
    [SerializeField] float smoothTime = .25f;

    // offset between camera and target
    private Vector3 offset;

    // This value will change at the runtime depending on target movement. Initialize with zero vector.
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        offset = transform.position - playerTransform.position;
    }

    private void LateUpdate()
    {
        transform.LookAt(aimTransform);
        Vector3 camOffset = playerTransform.position - aimTransform.position;
        Vector3 targetPosition = playerTransform.position + camOffset + new Vector3(0f, 6f, 0f);
        if (targetPosition.y < 2f) {
            targetPosition.y = 2f;
        }
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
