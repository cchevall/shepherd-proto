using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float life = 100f;
    [SerializeField] int scorePoints = 100;
    [SerializeField] GameObject explosionPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            TakeHit(100);
        }
    }

    private void TakeHit(float amount)
    {
        life -= amount;
        if (life <= 0f)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
            if (ScoreManager.isLoaded()) {
                ScoreManager.Instance.AddOrRemovePointsToScore(scorePoints);
            }
        }
    }
}
