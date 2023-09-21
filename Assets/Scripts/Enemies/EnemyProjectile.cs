using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damageOnCollision {
        get {
            return _damageOnCollision;
        }
    }
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] Transform explosionTransform;
    private float _projectileSpeed = 50f;
    private int _damageOnCollision = 25;

    // Update is called once per frame
    void Update()
    {
        SeekPlayerAndDestroy();
    }

    private void SeekPlayerAndDestroy()
    {
        transform.parent.Translate(Vector3.back * Time.deltaTime * GameManager.Instance.gameSpeed, Space.World);
        transform.parent.Translate(Vector3.forward * Time.deltaTime * _projectileSpeed, Space.Self);
        if (transform.parent.position.y < LevelConfig.projectilesBottomBound) {
            transform.parent.position = new Vector3(transform.parent.position.x, LevelConfig.projectilesBottomBound, transform.parent.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IHealth>().ApplyDamage(damageOnCollision);
            Explode();
        } else if (!other.CompareTag("Untagged") && !other.CompareTag("Enemy")) {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(explosionPrefab, explosionTransform.position, explosionTransform.rotation);
        Destroy(transform.parent.gameObject);
    }
}
