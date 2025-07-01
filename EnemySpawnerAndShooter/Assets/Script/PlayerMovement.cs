using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent = null;
    public static List<GameObject> EnemyList = new List<GameObject>();
    public static GameObject NearestEnemy;
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _agent.SetDestination(hit.point);
            }
        }
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
        CleanupNullEnemies();

        nearestDistance = Mathf.Infinity;

        if (EnemyList.Count == 0)
        {
            NearestEnemy = null;
            return;
        }

        GameObject nearest = null;

        for (int i = 0; i < EnemyList.Count; i++)
        {
            float distance = Vector3.Distance(this.transform.position, EnemyList[i].transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                NearestEnemy = EnemyList[i];
            }
        }
    }


    private void CleanupNullEnemies()
    {
        for (int i = EnemyList.Count - 1; i >= 0; i--)
        {
            if (EnemyList[i] == null)
            {
                EnemyList.RemoveAt(i);
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

