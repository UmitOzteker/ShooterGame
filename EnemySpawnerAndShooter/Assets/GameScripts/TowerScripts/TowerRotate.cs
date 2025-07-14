using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRotate : MonoBehaviour
{
    public GameObject NearestEnemy;
    float distance;
    float nearestDistance = 10000;

    // Start is called before the first frame update
    void Start()
    {
        NearestEnemy = EnemyManager.findNearestEnemy(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        NearestEnemy = EnemyManager.findNearestEnemy(transform.position);
        if (NearestEnemy != null)
        {
            Vector3 lookPos = new Vector3(
                NearestEnemy.transform.position.x,
                transform.position.y,
                NearestEnemy.transform.position.z
            );

            transform.LookAt(lookPos);
        }
    }
}
