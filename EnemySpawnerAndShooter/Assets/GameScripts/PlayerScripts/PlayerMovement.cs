using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent = null;
    public PlayerData selectedCharacter;
    public GameObject NearestEnemy;
    float distance;
    float nearestDistance = 10000;
    private Animator animator;

    void Awake()
    {
        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        NearestEnemy = EnemyManager.findNearestEnemy(transform.position);
    }

    bool isWalking = false;

    void Update()
    {
        if (_agent != null && !_agent.isOnNavMesh)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _agent.SetDestination(hit.point);
                if (animator != null)
                {
                    isWalking = true;
                    animator.SetBool("isWalking", isWalking);
                }
            }
        }

        NearestEnemy = EnemyManager.findNearestEnemy(transform.position);
        bool enemyVisible = false;
        if (NearestEnemy != null)
        {
            Vector3 lookPos = new Vector3(
                NearestEnemy.transform.position.x,
                transform.position.y,
                NearestEnemy.transform.position.z
            );
            transform.LookAt(lookPos);
            enemyVisible = true;
        }

        if (enemyVisible && !isWalking)
        {
            animator.SetBool("isSeenEnemyWhileStanding", enemyVisible);
        }

        if (animator != null)
        {
            animator.SetBool("isSeenEnemy", enemyVisible);
        }

        // Hedefe ulaştıysa idle'a geç - Basit çözüm
        if (animator != null && _agent != null)
        {
            if (
                !_agent.pathPending
                && _agent.remainingDistance <= 0.5f
                && _agent.velocity.sqrMagnitude < 1f
            )
            {
                isWalking = false;
                animator.SetBool("isWalking", isWalking);
            }
        }
    }
}
