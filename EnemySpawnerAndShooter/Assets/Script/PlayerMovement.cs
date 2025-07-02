using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent = null;
    public GameObject NearestEnemy;
    float distance;
    float nearestDistance = 10000;

    // Start is called before the first frame update
    void Start()
    {
        NearestEnemy = EnemyManager.findNearestEnemy(transform.position);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _agent.SetDestination(hit.point);
            }
        }
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
