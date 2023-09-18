using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // OffLimit Environment
    [SerializeField] GameObject mountainPrefab;
    [SerializeField] GameObject treePrefab;
    [SerializeField] List<GameObject> cloudPrefabs;
    [SerializeField] List<GameObject> grassPrefabs;
    [SerializeField] List<GameObject> housePrefabs;
    [SerializeField] GameObject rockPrefab;
    [SerializeField] GameObject shipPrefab;
    [SerializeField] GameObject enemyPrefab;

    private float xAxisBound = 100f;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(SpawnObstaclesRoutine());
        StartCoroutine(SpawnEnemiesRoutine());
        SpawnEnvironment();
        SpawnHousesAndGardens();
    }

    // Spawns Off Limit Environment and Static obstacles
    private void SpawnEnvironment()
    {
        SpawnMountains();
        SpawnClouds();
        SpawnGrass();
        SpawnForest();
    }

    private void SpawnClouds()
    {
        StartCoroutine(SpawnCloudsRoutine());
    }

    private void SpawnMountains() {
        StartCoroutine(SpawnMountainsRoutine());
    }

    private void SpawnGrass() {
        StartCoroutine(SpawnGrassRoutine());
    }

    private void SpawnForest()
    {
        StartCoroutine(SpawnForestRoutine());
    }

    private void SpawnHousesAndGardens()
    {
        StartCoroutine(SpawnHousesAndGardensRoutine());
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

    private void SpawnTree()
    {

    }

    /**
     * <=== Coroutines
     */
    IEnumerator SpawnForestRoutine()
    {
        while (!GameManager.Instance.isGameOver)
        {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            int count = 4;
            float treeDistance = 20f;
            float randomDistance = Random.Range(treeDistance - 5f, treeDistance + 5f);
            float treeXOffset = (LevelConfig.xBound - LevelConfig.xForestBound) / count;
            for (int i = 0; i < count; i++)
            {
                float xOffset = i * treeXOffset + Random.Range(0f, 40f);
                float xPos = LevelConfig.xBound - xOffset;
                float zPos = treeDistance + LevelConfig.offLimitZPos;
                Vector3 leftPos = new Vector3(-xPos, treePrefab.transform.position.y, zPos);
                Vector3 rightPos = new Vector3(xPos, treePrefab.transform.position.y, zPos);
                Instantiate(treePrefab, leftPos, GetRandomYRotation());
                Instantiate(treePrefab, rightPos, GetRandomYRotation());
            }
            yield return new WaitForSeconds(GetWaitTimeFromDepth(randomDistance));
        }
    }

    IEnumerator SpawnHousesAndGardensRoutine()
    {
        while (!GameManager.Instance.isGameOver)
        {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            int count = 4;
            float houseDepth = 47f;
            float randomDistance = Random.Range(houseDepth * 2f, houseDepth * 3f);
            float range = LevelConfig.xForestBound * 2;
            float box = range / count;
            float currentPos = LevelConfig.xForestBound - houseDepth;
            for (int i = 0; i < count; i++)
            {
                if (!isLucky(70f)) {
                    currentPos -= box;
                    continue ;
                }
                float houseXOffset = Random.Range(5f, 15f);
                float xPos = currentPos + houseXOffset;
                float zPos = Random.Range(LevelConfig.offLimitZPos - 30f, LevelConfig.offLimitZPos + 30f);
                int randIndex = Random.Range(0, housePrefabs.Count);
                GameObject prefab = housePrefabs[randIndex];
                Vector3 prefabPos = new Vector3(xPos, prefab.transform.position.y, zPos);
                Instantiate(prefab, prefabPos, prefab.transform.rotation);
                currentPos -= box;
            }
            yield return new WaitForSeconds(GetWaitTimeFromDepth(randomDistance));
        }
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            SpawnStdEnemies();
            SpawnShips();
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator SpawnMountainsRoutine()
    {
        while (!GameManager.Instance.isGameOver)
        {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            float mountainDepth = 450f;
            float randomDistance = Random.Range(mountainDepth, mountainDepth + mountainDepth / 2f);
            float randomHeight = Random.Range(-45f, 0f);
            float xPos = LevelConfig.xBound + mountainDepth * .7f;
            float zPos = mountainDepth + LevelConfig.offLimitZPos;
            Vector3 leftMountainPos = new Vector3(-xPos, randomHeight, zPos);
            Vector3 rightMountainPos = new Vector3(xPos, randomHeight, zPos);
            Instantiate(mountainPrefab, leftMountainPos, GetRandomYRotation());
            Instantiate(mountainPrefab, rightMountainPos, GetRandomYRotation());
            yield return new WaitForSeconds(GetWaitTimeFromDepth(randomDistance));
        }
    }

    IEnumerator SpawnGrassRoutine()
    {
        while (!GameManager.Instance.isGameOver)
        {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            float grassDepth = 110f;
            float randomDistance = Random.Range(grassDepth / 2f, grassDepth * 1.5f);
            float xPos = LevelConfig.xBound - grassDepth / 1.5f;
            float zPos = grassDepth + LevelConfig.offLimitZPos;
            int randIndex = Random.Range(0, grassPrefabs.Count);
            GameObject prefab = grassPrefabs[randIndex];
            Vector3 leftPos = new Vector3(-xPos, prefab.transform.position.y, zPos);
            Vector3 rightPos = new Vector3(xPos, prefab.transform.position.y, zPos);
            Instantiate(prefab, leftPos, GetRandomYRotation());
            Instantiate(prefab, rightPos, GetRandomYRotation());
            yield return new WaitForSeconds(GetWaitTimeFromDepth(randomDistance));
        }
    }

    IEnumerator SpawnCloudsRoutine()
    {
        while (!GameManager.Instance.isGameOver)
        {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            float cloudsDistance = 60f;
            float randomDistance = Random.Range(cloudsDistance / 2f, cloudsDistance * 1.5f);
            float range = LevelConfig.xBound * 1.5f;
            float xPos = Random.Range(-range, range);
            float zPos = LevelConfig.offLimitZPos;
            float yPos = Random.Range(200f, 350f);
            int randIndex = Random.Range(0, cloudPrefabs.Count);
            GameObject prefab = cloudPrefabs[randIndex];
            Vector3 cloudPos = new Vector3(-xPos, yPos, zPos);
            Instantiate(prefab, cloudPos, prefab.transform.rotation);
            yield return new WaitForSeconds(GetWaitTimeFromDepth(randomDistance));
        }
    }

    /**
     * Coroutines ===>
     */

    /**
     * <=== Helpers
     */

    bool isLucky(float percentage)
    {
        return Random.Range(0f, 100f) < percentage;
    }

    Quaternion GetRandomYRotation()
    {
        return Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));
    }

    float GetWaitTimeFromDepth(float depth)
    {
        return depth / GameManager.Instance.gameSpeed;
    }

    Vector3 GetAirPosition(float zOffset)
    {
        float randomXPos = Random.Range(-xAxisBound, xAxisBound);
        float randomYPos = Random.Range(30f, LevelConfig.yTopBound);
        return new Vector3(randomXPos, randomYPos, transform.position.z + zOffset);
    }

    /**
     * Helpers ===>
     */
}
