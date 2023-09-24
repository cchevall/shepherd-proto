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
    [SerializeField] List<GameObject> groundPrefabs;
    [SerializeField] GameObject tankPrefab;
    [SerializeField] GameObject rockPrefab;
    [SerializeField] GameObject shipPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject healthPrefab;

    private float xAxisBound = 100f;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(SpawnObstaclesRoutine());
        StartCoroutine(SpawnEnemiesRoutine());
        StartCoroutine(SpawnShipsRoutine());
        SpawnEnvironment();
        SpawnGroundArena();
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
        StartCoroutine(SpawnHealthRoutine());
    }

    private void SpawnGroundArena()
    {
        StartCoroutine(SpawnGroundArenaRoutine());
    }

    private void SpawnShips()
    {
        float randOffset = Random.Range(60f, 90f);
        float randomXPos = Random.Range(-xAxisBound, xAxisBound);
        float randomYPos = Random.Range(LevelConfig.yTopBound, LevelConfig.yTopBound + 30f);
        Vector3 airPos = new Vector3(randomXPos, randomYPos, transform.position.z + randOffset);
        Instantiate(shipPrefab, airPos, shipPrefab.transform.rotation);
    }

    private void SpawnStdEnemies()
    {
        int count = Random.Range(1, 3);
        for (int i = 0; i < count; i++)
        {
            float randOffset = Random.Range(10f, 50f);
            float randomXPos = Random.Range(-xAxisBound, xAxisBound);
            float randomYPos = Random.Range(40f, LevelConfig.yTopBound);
            Vector3 airPos = new Vector3(randomXPos, randomYPos, transform.position.z + randOffset);
            Instantiate(enemyPrefab, airPos, enemyPrefab.transform.rotation);
        }
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
            float treeDistance = 25f;
            float randomDistance = Random.Range(treeDistance - 5f, treeDistance + 5f);
            float treeXOffset = (LevelConfig.xBound - LevelConfig.xForestBound) / count;
            for (int i = 0; i < count; i++)
            {
                float xOffset = i * treeXOffset + Random.Range(0f, treeXOffset);
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

    IEnumerator SpawnHealthRoutine()
    {
        while (!GameManager.Instance.isGameOver) {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            float minDistance = GameManager.Instance.gameSpeed * 1;
            float maxDistance = GameManager.Instance.gameSpeed * 3;
            float randomDistance = Random.Range(minDistance, maxDistance);
            float yPos = Random.Range(LevelConfig.yBottomBound, LevelConfig.yTopBound);
            float xPos = Random.Range(LevelConfig.xForestBound, LevelConfig.xBound);
            float zPos = LevelConfig.offLimitZPos;
            Vector3 leftPos = new Vector3(-xPos, yPos, zPos);
            Vector3 rightPos = new Vector3(xPos, yPos, zPos);
            if (Physics.CheckSphere(leftPos, healthPrefab.GetComponent<SphereCollider>().radius))
            {
                Instantiate(healthPrefab, leftPos, healthPrefab.transform.rotation);
            }
            if (Physics.CheckSphere(rightPos, healthPrefab.GetComponent<SphereCollider>().radius))
            {
                Instantiate(healthPrefab, rightPos, healthPrefab.transform.rotation);
            }
            yield return new WaitForSeconds(GetWaitTimeFromDepth(randomDistance));
        }
    }

    IEnumerator SpawnGroundArenaRoutine()
    {
        while (!GameManager.Instance.isGameOver)
        {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            int count = isLucky(70f) ? 4 : 3;
            float houseDepth = 47f;
            float randomDistance = Random.Range(houseDepth * 2f, houseDepth * 3f);
            float range = LevelConfig.xForestBound * 2;
            float box = range / count;
            float currentPos = LevelConfig.xForestBound - houseDepth;
            for (int i = 0; i < count; i++)
            {
                float houseXOffset = Random.Range(5f, 15f);
                float xPos = currentPos + houseXOffset;
                float zPos = Random.Range(LevelConfig.offLimitZPos - 30f, LevelConfig.offLimitZPos + 30f);
                int randIndex = Random.Range(0, groundPrefabs.Count);
                GameObject prefab = groundPrefabs[randIndex];
                Vector3 prefabPos = new Vector3(xPos, prefab.transform.position.y, zPos);
                Instantiate(prefab, prefabPos, prefab.transform.rotation);
                currentPos -= box;
            }
            yield return new WaitForSeconds(GetWaitTimeFromDepth(randomDistance));
        }
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (!GameManager.Instance.isGameOver)
        {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            SpawnStdEnemies();
            yield return new WaitForSeconds(Random.Range(1f, 5f));
        }
    }

    IEnumerator SpawnShipsRoutine()
    {
        while (!GameManager.Instance.isGameOver)
        {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            SpawnShips();
            yield return new WaitForSeconds(Random.Range(5f, 10f));
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

    /**
     * Helpers ===>
     */
}
