using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MovementDirection {
    Forward,
    Backward,
    WorldForward,
    WorldBackward,
}

public class Move : MonoBehaviour
{
    [SerializeField] float additionalSpeed = 0f;
    [SerializeField] MovementDirection moveDirection = MovementDirection.WorldBackward;

    // Update is called once per frame
    void Update()
    {
        MoveObject();
    }

    private void MoveObject()
    {
        if (GameManager.isLoaded() && (!GameManager.Instance.isStarted || GameManager.Instance.isGameOver || GameManager.Instance.isPaused))
        {
            return;
        }
        float finalSpeed = GameManager.Instance.gameSpeed + additionalSpeed;
        switch (moveDirection)
        {
            case MovementDirection.WorldBackward:
                transform.Translate(-Vector3.forward * Time.deltaTime * finalSpeed, Space.World);
                return ;
            case MovementDirection.WorldForward:
                transform.Translate(Vector3.forward * Time.deltaTime * finalSpeed, Space.World);
                return ;
            case MovementDirection.Forward:
                transform.Translate(Vector3.forward * Time.deltaTime * finalSpeed, Space.Self);
                return ;
            case MovementDirection.Backward:
                transform.Translate(-Vector3.forward * Time.deltaTime * finalSpeed, Space.Self);
                return ;
            default:
                transform.Translate(-Vector3.forward * Time.deltaTime * finalSpeed, Space.World);
                return ;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.CompareTag("Obstacle"))
        {
            return;
        }
        if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
        }
    }
}
