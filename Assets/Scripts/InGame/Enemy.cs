using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float life = 100f;

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
        if (life >= 0f)
        {
            Destroy(gameObject);
        }
    }
}
