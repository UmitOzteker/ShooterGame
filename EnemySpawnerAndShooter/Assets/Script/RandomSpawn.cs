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
    private int spawnedCubeCount = 0;
    private List<Vector3> spawnedPositions = new List<Vector3>();

    private float spawnInterval = 5f;    // 5 saniyede bir spawn 
    private float minSpawnDistance = 5f; // En az 5 birim uzaða spawn
    private int maxCubeCount = 15;       // Maksimum Enemy sayýsý
    private float minDistanceBetweenCubes = 2f; 
    private bool tooClose = false;

    void Start()
    {
        planeSize = plane.GetComponent<MeshRenderer>().bounds.size;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && spawnedCubeCount < maxCubeCount)
        {
            Vector3 randomSpawnPosition = GetValidSpawnPosition();

            if (randomSpawnPosition != Vector3.zero) // Uygun pozisyon bulunduysa
            {
                Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);
                spawnedCubeCount++;
                spawnTimer = 0f;
            }
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
       
            float x = Random.Range(-planeSize.x / 2, planeSize.x / 2);
            float z = Random.Range(-planeSize.z / 2, planeSize.z / 2);
            Vector3 spawnPos = new Vector3(x, 1, z);

        foreach (var pos in spawnedPositions)
        {
            if (Vector3.Distance(spawnPos, pos) < minDistanceBetweenCubes)
            {
                tooClose = true;
                break;
            }
        }

        if (Vector3.Distance(spawnPos, player.position) >= minSpawnDistance && !tooClose)
            {
                return spawnPos;
            }

        

        return Vector3.zero; // Uygun pozisyon bulunamazsa spawn yapma
    }
}
