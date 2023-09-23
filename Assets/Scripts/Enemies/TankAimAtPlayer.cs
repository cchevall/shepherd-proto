using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAimAtPlayer : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPos;
    private float _zPositionBound = 75f;

    private Transform playerTransform;

    // Update is called once per frame
    void LateUpdate()
    {
        LookAtPlayer();
    }

    /**
     * Update Tank canon rotation to look at player position
     */
    private void LookAtPlayer()
    {
        if (playerTransform == null || transform.position.z < _zPositionBound) {
            return ;
        }
        Vector3 fixedOnYPlayerPos = playerTransform.position;
        fixedOnYPlayerPos.y = transform.position.y;
        transform.LookAt(fixedOnYPlayerPos);
    }

    /**
     * Instantiate a projectile looking at player position
     */
    private void Shoot()
    {
        if (transform.position.z < _zPositionBound || GameManager.Instance.isGameOver) {
            return ;
        }
        Transform targetRotation = transform;
        targetRotation.LookAt(playerTransform);
        Instantiate(projectilePrefab, projectileSpawnPos.position, targetRotation.rotation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            StartCoroutine(ShootCoroutine());
        }
    }

    private IEnumerator ShootCoroutine()
    {
        while (!GameManager.Instance.isGameOver && transform.position.z > _zPositionBound)
        {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            float randTimeBetweenProjectiles = Random.Range(1f, 2.5f);
            yield return new WaitForSeconds(randTimeBetweenProjectiles);
            Shoot();
        }
    }
}
