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
        findNearestEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        findNearestEnemy();
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
    public void findNearestEnemy()
    {
        nearestDistance = Mathf.Infinity;

        if (PlayerMovement.EnemyList.Count == 0)
        {
            NearestEnemy = null;
            return;
        }

        GameObject nearest = null;

        for (int i = 0; i < PlayerMovement.EnemyList.Count; i++)
        {
            float distance = Vector3.Distance(this.transform.position, PlayerMovement.EnemyList[i].transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                NearestEnemy = PlayerMovement.EnemyList[i];
            }
        }
    }
}
