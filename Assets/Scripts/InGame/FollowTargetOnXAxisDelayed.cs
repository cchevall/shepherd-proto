using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This has huge wobbling (gliches) issues.
public class FollowTargetOnXAxisDelayed : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The transform to follow")]
    public GameObject target;

    private Vector3 initialPosition;

    private struct PointInSpace
    {
        public Vector3 Position;
        public float Time;
    }

    //[SerializeField]
    //[Tooltip("The transform to follow")]
    //public GameObject target;

    [SerializeField]
    [Tooltip("The offset between the target and the camera")]
    private Vector3 offset;

    [Tooltip("The delay before the camera starts to follow the target")]
    [SerializeField]
    private float delay = 0.3f;

    [SerializeField]
    [Tooltip("The speed used in the lerp function when the camera follows the target")]
    private float speed = 7;

    ///<summary>
    /// Contains the positions of the target for the last X seconds
    ///</summary>
    private Queue<PointInSpace> pointsInSpace = new Queue<PointInSpace>();

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 newPosition = new Vector3(target.transform.position.x, target.transform.position.y, initialPosition.z);
        // Add the current target position to the list of positions
        pointsInSpace.Enqueue(new PointInSpace() { Position = newPosition, Time = Time.time });

        // Move the camera to the position of the target X seconds ago 
        while (pointsInSpace.Count > 0 && pointsInSpace.Peek().Time <= Time.time - delay + Mathf.Epsilon)
        {
            transform.position = Vector3.Lerp(transform.position, pointsInSpace.Dequeue().Position + offset, Time.deltaTime * speed);
        }
    }
}
