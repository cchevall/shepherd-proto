using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
    public int currentHP {
        get {
            return _currentHP;
        }
    }
    public int damageOnCollision {
        get {
            return _damageOnCollision;
        }
    }
    [SerializeField] int _currentHP = 100;
    [SerializeField] int _damageOnCollision = 15;

    [SerializeField] int scorePoints = 100;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] Transform explosionTransform;

    public void ApplyDamage(int amount) {
        _currentHP -= amount;
        if (_currentHP <= 0)
        {
            Instantiate(explosionPrefab, explosionTransform.position, explosionTransform.rotation);
            Destroy(transform.parent.gameObject);
            if (ScoreManager.isLoaded()) {
                ScoreManager.Instance.AddOrRemovePointsToScore(scorePoints);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Enemy collided with {other.tag}, apply {damageOnCollision} damage");
            other.GetComponent<IHealth>().ApplyDamage(damageOnCollision);
        }
    }
}
