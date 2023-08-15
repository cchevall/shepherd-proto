using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetOnXAxis : MonoBehaviour
{
    public GameObject target;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = new Vector3(target.transform.position.x, target.transform.position.y, initialPosition.z);
        transform.position = newPosition;
    }
}