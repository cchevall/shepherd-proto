using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] GameObject sparklePrefab;
    private int healthPoints = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collided with health");
            ApplyPowerUp(other);
        }
    }

    private void ApplyPowerUp(Collider player)
    {
        player.GetComponent<PlayerController>().ApplyHeal(healthPoints);
        Instantiate(sparklePrefab, transform.position, sparklePrefab.transform.rotation);
        Destroy(gameObject);
    }
}
