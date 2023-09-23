using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] protected Vector3 axis = Vector3.up;
    [SerializeField] protected float rate;

    void Update()
    {
        RotateSelf();
    }

    protected void RotateSelf()
    {
        transform.localRotation = Quaternion.AngleAxis(Time.time * rate * 360, axis);
    }
}
