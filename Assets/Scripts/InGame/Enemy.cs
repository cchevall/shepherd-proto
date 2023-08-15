using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float life = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
