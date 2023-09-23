using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekPlayer : MonoBehaviour
{
    private Transform playerTransform;
    private float randomNoiseOnYAxis = 0f;
    private float randomNoiseOnXAxis = 0f;
    private float seekNoise = 11f;
    private float smoothTime = 1.5f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        StartCoroutine(UpdateRandomNoiseOnAxis());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null) {
            RotateToPlayerWithNoise();
        }
    }

    private void RotateToPlayerWithNoise()
    {
        if (transform.position.z > 50f) {
            float yTargetPos = playerTransform.position.y + randomNoiseOnYAxis;
            float xTargetPos = playerTransform.position.x + randomNoiseOnXAxis;
            float zTargetPos = playerTransform.position.z + GameManager.Instance.gameSpeed;
            yTargetPos = yTargetPos < LevelConfig.projectilesBottomBound ? LevelConfig.projectilesBottomBound : yTargetPos;
            Vector3 targetPosition = new Vector3(xTargetPos, yTargetPos, zTargetPos);
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            transform.LookAt(smoothedPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
        }
    }

    IEnumerator UpdateRandomNoiseOnAxis()
    {
        while(!GameManager.Instance.isGameOver && transform.position.z > 50f)
        {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            yield return new WaitForSeconds(smoothTime);
            randomNoiseOnYAxis = Random.Range(-seekNoise, seekNoise);
            randomNoiseOnXAxis = Random.Range(-seekNoise, seekNoise);
        }
    }
}
