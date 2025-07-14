using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [Header("Spawn Ayarları")]
    public GameObject plane;
    public GameObject[] enemyPrefabs; // Birden fazla düşman prefab'ı
    public Transform player;

    private Vector3 planeSize;
    private float spawnTimer = 0f;
    private List<Vector3> spawnedPositions = new List<Vector3>();
    public static List<GameObject> EnemyList = new List<GameObject>();

    public float spawnInterval = 5f; // 5 saniyede bir spawn
    private float minSpawnDistance = 5f; // En az 5 birim uzağa spawn
    private int maxCubeCount = 15; // Maksimum Enemy sayısı
    private float minDistanceBetweenCubes = 2f;
    private int maxSpawnAttempts = 50;

    void Start()
    {
        if (plane != null)
        {
            planeSize = plane.GetComponent<MeshRenderer>().bounds.size;
        }

        // Player otomatik bul
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (EnemyList == null)
        {
            return;
        }

        // Aktif düşman sayısını kontrol et
        int activeEnemyCount = 0;
        for (int i = EnemyList.Count - 1; i >= 0; i--)
        {
            if (EnemyList[i] == null)
            {
                EnemyList.RemoveAt(i);
            }
            else
            {
                activeEnemyCount++;
            }
        }

        if (spawnTimer >= spawnInterval && activeEnemyCount < maxCubeCount)
        {
            Vector3 randomSpawnPosition = GetValidSpawnPosition();

            if (randomSpawnPosition != Vector3.zero)
            {
                // Rastgele düşman seç
                GameObject randomEnemyPrefab = GetRandomEnemyPrefab();

                if (randomEnemyPrefab != null)
                {
                    GameObject newEnemy = Instantiate(
                        randomEnemyPrefab,
                        randomSpawnPosition,
                        Quaternion.identity
                    );

                    if (newEnemy != null)
                    {
                        AddEnemy(newEnemy);
                        spawnedPositions.Add(randomSpawnPosition);
                        spawnTimer = 0f;
                    }
                }
            }
        }

        CleanUpDeadEnemyPositions();
    }

    private GameObject GetRandomEnemyPrefab()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            return null;
        }

        // Rastgele düşman seç
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[randomIndex];
    }

    private Vector3 GetValidSpawnPosition()
    {
        if (player == null)
            return Vector3.zero;

        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            float x = Random.Range(-planeSize.x / 2, planeSize.x / 2);
            float z = Random.Range(-planeSize.z / 2, planeSize.z / 2);
            Vector3 spawnPos = new Vector3(x, 1, z);

            // Player'dan uzaklık kontrolü
            if (Vector3.Distance(spawnPos, player.position) < minSpawnDistance)
            {
                continue;
            }

            // Diğer düşmanlardan uzaklık kontrolü
            bool tooClose = false;
            foreach (var pos in spawnedPositions)
            {
                if (Vector3.Distance(spawnPos, pos) < minDistanceBetweenCubes)
                {
                    tooClose = true;
                    break;
                }
            }

            // Aktif düşmanlardan uzaklık kontrolü
            if (!tooClose && EnemyList != null)
            {
                foreach (GameObject enemy in EnemyList)
                {
                    if (
                        enemy != null
                        && Vector3.Distance(spawnPos, enemy.transform.position)
                            < minDistanceBetweenCubes
                    )
                    {
                        tooClose = true;
                        break;
                    }
                }
            }

            if (!tooClose)
            {
                return spawnPos;
            }
        }

        return Vector3.zero;
    }

    private void CleanUpDeadEnemyPositions()
    {
        if (EnemyList == null)
            return;

        for (int i = spawnedPositions.Count - 1; i >= 0; i--)
        {
            bool foundActiveEnemy = false;

            foreach (GameObject enemy in EnemyList)
            {
                if (
                    enemy != null
                    && Vector3.Distance(enemy.transform.position, spawnedPositions[i]) < 1f
                )
                {
                    foundActiveEnemy = true;
                    break;
                }
            }

            if (!foundActiveEnemy)
            {
                spawnedPositions.RemoveAt(i);
            }
        }
    }

    public static void AddEnemy(GameObject enemy)
    {
        if (!EnemyList.Contains(enemy))
        {
            EnemyList.Add(enemy);
        }
    }

    public static void RemoveEnemy(GameObject enemy)
    {
        EnemyList.Remove(enemy);
    }
}
