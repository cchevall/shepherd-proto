using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // OffLimit Environment
    [SerializeField] GameObject fogWallPrefab;
    [SerializeField] GameObject mountainPrefab;
    [SerializeField] GameObject treePrefab;
    [SerializeField] List<GameObject> grassPrefabs;
    [SerializeField] GameObject rockPrefab;
    [SerializeField] GameObject shipPrefab;
    [SerializeField] GameObject enemyPrefab;

    private float xAxisBound = 100f;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(SpawnObstaclesRoutine());
        // StartCoroutine(SpawnEnemiesRoutine());
        SpawnEnvironment();
    }

    // Spawns Off Limit Environment and Static obstacles
    private void SpawnEnvironment()
    {
        SpawnFogWall();
        SpawnMountains();
        SpawnGrass();
        SpawnTrees();
    }

    private void SpawnFogWall() {
        Vector3 fogWallPostion = Vector3.zero;
        fogWallPostion.z = LevelConfig.offLimitZPos;
        Instantiate(fogWallPrefab, fogWallPostion, fogWallPrefab.transform.rotation);
    }

    private void SpawnMountains() {
        StartCoroutine(SpawnMountainsRoutine());
    }

    private void SpawnGrass() {
        StartCoroutine(SpawnGrassRoutine());
    }

    private void SpawnTrees()
    {
        StartCoroutine(SpawnTreesRoutine());
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

    /**
     * <=== Coroutines
     */
    IEnumerator SpawnTreesRoutine()
    {
        while (!GameManager.Instance.isGameOver)
        {
            if (GameManager.Instance.isPaused) {
                continue;
            }
            int count = 4;
            float treeDepth = 20f;
            float randomDistance = Random.Range(treeDepth - 5f, treeDepth + 5f);
            float treeXOffset = (LevelConfig.xBound - LevelConfig.xForestBound) / count;
            for (int i = 0; i < count; i++)
            {
                float xOffset = i * treeXOffset + Random.Range(0f, 40f);
                float xPos = LevelConfig.xBound - xOffset;
                float zPos = treeDepth + LevelConfig.offLimitZPos;
                Vector3 leftPos = new Vector3(-xPos, treePrefab.transform.position.y, zPos);
                Vector3 rightPos = new Vector3(xPos, treePrefab.transform.position.y, zPos);
                Instantiate(treePrefab, leftPos, GetRandomYRotation());
                Instantiate(treePrefab, rightPos, GetRandomYRotation());
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
            float xPos = LevelConfig.xBound + mountainDepth / 1.5f;
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
    /**
     * Coroutines ===>
     */

    /**
     * <=== Helpers
     */

    Quaternion GetRandomYRotation()
    {
        return Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));
    }

    float GetWaitTimeFromDepth(float depth)
    {
        return depth / GameManager.Instance.gameSpeed;
    }

    Vector3 GetGroundPosition(Transform prefabPos, float zOffset, float? xRange)
    {
        xRange = (xRange.HasValue ? xRange : xAxisBound);
        float randomXPos = Random.Range((float) -xRange, (float) xRange);
        return new Vector3(randomXPos, prefabPos.position.y, transform.position.z + zOffset);
    }

    Vector3 GetAirPosition(float zOffset)
    {
        float randomXPos = Random.Range(-xAxisBound, xAxisBound);
        float randomYPos = Random.Range(5f, LevelConfig.yTopBound);
        return new Vector3(randomXPos, randomYPos, transform.position.z + zOffset);
    }

    Vector3 GetBoundPosition(float zOffset)
    {
        float xBound = 35f;
        float yBound = 25f;
        float randomXPos = Random.Range(-xBound, xBound);
        float randomYPos = Random.Range(1f, yBound);
        return new Vector3(randomXPos, randomYPos, transform.position.z + zOffset);
    }
    /**
     * Helpers ===>
     */
}
