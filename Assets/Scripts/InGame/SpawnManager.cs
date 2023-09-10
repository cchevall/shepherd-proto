using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject treePrefab;
    public GameObject rockPrefab;
    public GameObject shipPrefab;
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    private float spawnZPos = 450f;
    private float xAxisBound = 100f;
    private float yAxisTopBound = 50f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObstaclesRoutine());
        StartCoroutine(SpawnEnemiesRoutine());
        StartCoroutine(SpawnPowerupsRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 GetGroundPosition(Transform prefabPos, float zOffset, float? xRange)
    {
        xRange = (xRange.HasValue ? xRange : xAxisBound);
        float randomXPos = Random.Range((float) -xRange, (float) xRange);
        return new Vector3(randomXPos, prefabPos.position.y, spawnZPos + zOffset);
    }

    Vector3 GetAirPosition(float zOffset)
    {
        float randomXPos = Random.Range(-xAxisBound, xAxisBound);
        float randomYPos = Random.Range(5f, yAxisTopBound);
        return new Vector3(randomXPos, randomYPos, spawnZPos + zOffset);
    }

    Vector3 GetBoundPosition(float zOffset)
    {
        float xBound = 35f;
        float yBound = 25f;
        float randomXPos = Random.Range(-xBound, xBound);
        float randomYPos = Random.Range(1f, yBound);
        return new Vector3(randomXPos, randomYPos, spawnZPos + zOffset);
    }

    private void SpawnPowerups()
    {
        float randOffset = Random.Range(0f, 25f);
        float randomXPos = Random.Range(-50f, 50f);
        Vector3 position = new Vector3(randomXPos, 1f, spawnZPos + randOffset);
        Instantiate(powerupPrefab, position, powerupPrefab.transform.rotation);
    }

    private void SpawnTrees()
    {
        int count = Random.Range(7, 16);
        for (int i = 0; i < count; i++)
        {
            float randOffset = Random.Range(30f, 50f);
            Instantiate(treePrefab, GetGroundPosition(treePrefab.transform, randOffset, 300f), treePrefab.transform.rotation);
        }
    }

    private void SpawnRocks()
    {
        int count = Random.Range(3, 7);
        for (int i = 0; i < count; i++)
        {
            float randOffset = Random.Range(0f, 25f);
            Instantiate(rockPrefab, GetGroundPosition(rockPrefab.transform, randOffset, 300f), rockPrefab.transform.rotation);
        }
    }

    private void SpawnShips()
    {
        int count = Random.Range(1, 4);
        for (int i = 0; i < count; i++)
        {
            float randOffset = Random.Range(60f, 90f);
            Instantiate(shipPrefab, GetAirPosition(randOffset), shipPrefab.transform.rotation);
        }
    }

    private void SpawnStdEnemies()
    {
        int count = Random.Range(5, 7);
        for (int i = 0; i < count; i++)
        {
            float randOffset = Random.Range(10f, 50f);
            Instantiate(enemyPrefab, GetAirPosition(randOffset), enemyPrefab.transform.rotation);
        }
    }

    IEnumerator SpawnObstaclesRoutine()
    {
        while (true)
        {
            SpawnTrees();
            SpawnShips();
            yield return new WaitForSeconds(2f);
            SpawnRocks();
            SpawnShips();
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            SpawnStdEnemies();
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator SpawnPowerupsRoutine()
    {
        while (true)
        {
            float toWait = Random.Range(5f, 10f);
            yield return new WaitForSeconds(toWait);
            SpawnPowerups();
        }
    }
}
