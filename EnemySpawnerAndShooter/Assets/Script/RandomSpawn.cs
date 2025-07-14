using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject plane;
    public GameObject cubePrefab;
    public Transform player;

    private Vector3 planeSize;
    private float spawnTimer = 0f;
    private List<Vector3> spawnedPositions = new List<Vector3>();
    public static List<GameObject> EnemyList = new List<GameObject>();


    private float spawnInterval = 5f;    // 5 saniyede bir spawn
    private float minSpawnDistance = 5f; // En az 5 birim uzağa spawn
    private int maxCubeCount = 15;       // Maksimum Enemy sayısı
    private float minDistanceBetweenCubes = 2f;

    // Güvenlik için maksimum deneme sayısı
    private int maxSpawnAttempts = 50;

    void Start()
    {
        if (plane != null)
        {
            planeSize = plane.GetComponent<MeshRenderer>().bounds.size;
        }
        else
        {
            Debug.LogError("Plane GameObject atanmamış!");
        }

        // Player otomatik bul
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player bulunamadı!");
            }
        }
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        // Null check ekle
        if (EnemyList == null)
        {
            Debug.LogError("EnemyList null!");
            return;
        }

        // Aktif düşman sayısını kontrol et (null olanları say)
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

            if (randomSpawnPosition != Vector3.zero) // Uygun pozisyon bulunduysa
            {
                GameObject newEnemy = Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);

                // Null check
                if (newEnemy != null)
                {
                    AddEnemy(newEnemy);
                    spawnedPositions.Add(randomSpawnPosition);
                    spawnTimer = 0f;

                    Debug.Log($"Düşman spawn edildi. Toplam: {activeEnemyCount + 1}");
                }
            }
        }

        CleanUpDeadEnemyPositions(); // Ölen Düşman Pozisyonlarının Temizlenmesi
    }

    private Vector3 GetValidSpawnPosition()
    {
        // Player null kontrolü
        if (player == null) return Vector3.zero;

        // Maksimum deneme sayısı ile sonsuz döngüyü engelle
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            float x = Random.Range(-planeSize.x / 2, planeSize.x / 2);
            float z = Random.Range(-planeSize.z / 2, planeSize.z / 2);
            Vector3 spawnPos = new Vector3(x, 1, z);

            // Player'dan uzaklık kontrolü
            if (Vector3.Distance(spawnPos, player.position) < minSpawnDistance)
            {
                continue; // Bu pozisyon çok yakın, tekrar dene
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
                    if (enemy != null && Vector3.Distance(spawnPos, enemy.transform.position) < minDistanceBetweenCubes)
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

        return Vector3.zero; // Uygun pozisyon bulunamazsa
    }

    private void CleanUpDeadEnemyPositions()
    {
        if (EnemyList == null) return;

        for (int i = spawnedPositions.Count - 1; i >= 0; i--)
        {
            bool foundActiveEnemy = false;

            foreach (GameObject enemy in EnemyList)
            {
                if (enemy != null && Vector3.Distance(enemy.transform.position, spawnedPositions[i]) < 1f)
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

    // Düşman öldüğünde çağrılacak
    public static void RemoveEnemy(GameObject enemy)
    {
        EnemyList.Remove(enemy);
    }
}