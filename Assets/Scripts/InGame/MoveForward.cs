using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] float additionalSpeed = 0f;

    // Update is called once per frame
    void Update()
    {
        MoveObjectForward();
    }

    private void MoveObjectForward()
    {
        if (!GameManager.isLoaded())
        {
            return;
        }
        float finalSpeed = GameManager.Instance.gameSpeed + additionalSpeed;
        transform.Translate(Vector3.forward * Time.deltaTime * finalSpeed, Space.Self);
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
