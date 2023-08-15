using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float additionalSpeed = 0f;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveObjectForward();
    }

    private void MoveObjectForward()
    {
        float finalSpeed = gameManager.gameSpeed + additionalSpeed;
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
