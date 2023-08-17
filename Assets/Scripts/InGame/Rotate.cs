using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] Vector3 axis = Vector3.up;
    [SerializeField] float rate;

    void Update()
    {
        RotateSelf();
    }

    void RotateSelf()
    {
        transform.localRotation = Quaternion.AngleAxis(Time.time * rate * 360, axis);
    }
}
