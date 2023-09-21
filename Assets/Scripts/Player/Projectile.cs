using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damageOnCollision {
        get {
            return _damageOnCollision;
        }
    }
    [SerializeField] int _damageOnCollision = 100;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] Transform explosionTransform;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IHealth>().ApplyDamage(damageOnCollision);
            Debug.Log($"Projectile collided with {other.tag}");
            Explode();
        } else if (!other.CompareTag("Untagged") && !other.CompareTag("Player")) {
            Debug.Log($"Projectile collided with {other.tag}");
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosionPrefab, explosionTransform.position, explosionTransform.rotation);
        Destroy(transform.parent.gameObject);
    }
}
