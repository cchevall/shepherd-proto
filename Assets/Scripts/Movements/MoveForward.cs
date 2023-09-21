using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MoveDirection {
    Forward,
    Backward,
    WorldForward,
    WorldBackward,
}

public class MoveForward : MonoBehaviour
{
    [SerializeField] float additionalSpeed = 0f;
    [SerializeField] MoveDirection moveDirection = MoveDirection.WorldBackward;

    // Update is called once per frame
    void Update()
    {
        MoveObjectForward();
    }

    private void MoveObjectForward()
    {
        if (GameManager.isLoaded() && (!GameManager.Instance.isStarted || GameManager.Instance.isGameOver || GameManager.Instance.isPaused))
        {
            return;
        }
        float finalSpeed = GameManager.Instance.gameSpeed + additionalSpeed;
        switch (moveDirection)
        {
            case MoveDirection.WorldBackward:
                transform.Translate(-Vector3.forward * Time.deltaTime * finalSpeed, Space.World);
                return ;
            case MoveDirection.WorldForward:
                transform.Translate(Vector3.forward * Time.deltaTime * finalSpeed, Space.World);
                return ;
            case MoveDirection.Forward:
                transform.Translate(Vector3.forward * Time.deltaTime * finalSpeed, Space.Self);
                return ;
            case MoveDirection.Backward:
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
