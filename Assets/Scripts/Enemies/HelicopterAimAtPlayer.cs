using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterAimAtPlayer : MonoBehaviour
{
    [SerializeField] GameObject projectilePlaceHolder;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPos;
    private bool hasShot = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Shoot()
    {
        if (hasShot) {
            return ;
        }
        hasShot = true;
        projectilePlaceHolder.gameObject.SetActive(false);
        Instantiate(projectilePrefab, projectileSpawnPos.position, projectilePrefab.transform.rotation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Shoot();
        }
    }
}
