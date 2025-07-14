using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static GameObject findNearestEnemy(Vector3 fromPosition)
    {
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in RandomSpawn.EnemyList)
        {
            if (enemy == null)
                continue;

            float dist = Vector3.Distance(fromPosition, enemy.transform.position);
            if (dist < nearestDistance)
            {
                nearestDistance = dist;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    public static void CleanupNullEnemies()
    {
        for (int i = RandomSpawn.EnemyList.Count - 1; i >= 0; i--)
        {
            if (RandomSpawn.EnemyList[i] == null)
            {
                RandomSpawn.EnemyList.RemoveAt(i);
            }
        }
    }
}
