using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowCharacter : MonoBehaviour
{
    [Header("Düşman Verileri")]
    [SerializeField]
    EnemyData enemyData;

    [Header("Referanslar")]
    [SerializeField]
    GameObject player;

    [SerializeField]
    HealthBar healthBar;

    [SerializeField]
    Currency currencyScript;

    [Header("Kan Efekti")]
    [SerializeField]
    private GameObject bloodEffectPrefab;

    [SerializeField]
    private Transform bloodSpawnPoint;

    [Header("NavMesh Ayarları")]
    [SerializeField]
    private NavMeshAgent _agent = null;

    private float currentHealth;
    private float maxHealth;
    private float speed;
    private float minDistance;
    private float maxDistance;
    private float stopDistance;
    private int bloodMoneyAmount;
    private bool isDead = false;
    private bool isWalking = false;
    private Animator animator;
    private EnemyShooter enemyShooter;

    void Awake()
    {
        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
        enemyShooter = GetComponent<EnemyShooter>();
    }

    void Start()
    {
        if (enemyData != null)
        {
            InitializeFromEnemyData();
        }
        else
        {
            SetDefaultValues();
        }

        FindReferences();
        SetupNavMeshAgent();

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    void InitializeFromEnemyData()
    {
        maxHealth = enemyData.maxHealth;
        currentHealth = maxHealth;
        speed = enemyData.speed;
        minDistance = enemyData.minDistance;
        maxDistance = enemyData.maxDistance;
        stopDistance = enemyData.stopDistance;
        bloodMoneyAmount = Mathf.RoundToInt(enemyData.bloodMoneyAmount);
    }

    void SetDefaultValues()
    {
        maxHealth = 100f;
        currentHealth = 100f;
        speed = 3.5f;
        minDistance = 2f;
        maxDistance = 10f;
        stopDistance = 2f;
        bloodMoneyAmount = 50;
    }

    void SetupNavMeshAgent()
    {
        if (_agent != null)
        {
            _agent.speed = speed;
            _agent.stoppingDistance = stopDistance;
            _agent.acceleration = 8f;
            _agent.angularSpeed = 120f;

            // Çoklu platform/plane desteği için
            _agent.autoTraverseOffMeshLink = true; // OffMeshLink'leri otomatik geç
            _agent.autoBraking = true; // Hedefe yaklaşırken yavaşla
            _agent.autoRepath = true; // Yol bloklandığında yeniden hesapla

            // Yükseklik farklılıkları için
            _agent.baseOffset = 0f; // Zemin seviyesi
            _agent.height = 2f; // Karakter yüksekliği
            _agent.radius = 0.5f; // Çarpışma yarıçapı

            // Merdiven ve rampa geçişleri için
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
    }

    void FindReferences()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<HealthBar>();
        }

        if (currencyScript == null)
        {
            currencyScript = FindObjectOfType<Currency>();
        }
    }

    void Update()
    {
        if (player == null || isDead || _agent == null)
            return;

        // NavMesh üzerinde değilse işlem yapma
        if (!_agent.isOnNavMesh)
        {
            // Eğer NavMesh dışındaysa, en yakın NavMesh noktasını bul
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
            return;
        }

        // Oyuncuya olan mesafe hesapla
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Oyuncuya doğru hareket et
        MoveTowardsPlayer(distance);

        // Animasyon durumlarını güncelle
        UpdateAnimations(distance);
    }

    void MoveTowardsPlayer(float distance)
    {
        if (distance > stopDistance)
        {
            // Mesafeye göre hızı ayarla
            if (distance > maxDistance)
            {
                _agent.speed = speed * 2f; // Uzaksa hızlan
            }
            else
            {
                _agent.speed = speed;
            }

            // Oyuncuya doğru hareket et
            _agent.SetDestination(player.transform.position);
            isWalking = true;
        }
        else
        {
            // Durma mesafesine ulaştıysa dur
            _agent.ResetPath();
            isWalking = false;
        }

        // Player'a doğru bakma (Y ekseninde rotasyon)
        if (player != null)
        {
            Vector3 lookDirection = player.transform.position - transform.position;
            lookDirection.y = 0; // Y eksenini sıfırla, sadece yatay düzlemde bak

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    Time.deltaTime * 5f
                );
            }
        }
    }

    void UpdateAnimations(float distance)
    {
        if (animator == null)
            return;

        // Yürüme animasyonu
        animator.SetBool("isWalking", isWalking);

        // Oyuncuya yakınlık animasyonu
        bool isClosePlayer = distance <= stopDistance;
        animator.SetBool("isClosePlayer", isClosePlayer);

        // Oyuncuyu görme animasyonu
        bool isSeenEnemy = distance <= maxDistance;
        animator.SetBool("isSeenEnemy", isSeenEnemy);

        // Duruyorken düşman görme animasyonu
        if (isSeenEnemy && !isWalking)
        {
            animator.SetBool("isSeenEnemyWhileStanding", true);
        }
        else
        {
            animator.SetBool("isSeenEnemyWhileStanding", false);
        }
    }

    bool isDying = false;

    public void TakeMeteorDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        // Kan efekti göster
        ShowBloodEffect();

        // Health bar güncelle
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        // Eğer can 0 veya altına düştüyse öl
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead)
            return;

        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                currentHealth -= bullet.damage;
            }

            ShowBloodEffect();

            if (healthBar != null)
            {
                healthBar.UpdateHealthBar(currentHealth, maxHealth);
            }

            Destroy(other.gameObject);

            if (currentHealth <= 0 && !isDead)
            {
                Die();
            }
        }
    }

    void ShowBloodEffect()
    {
        if (bloodEffectPrefab != null && bloodSpawnPoint != null)
        {
            GameObject blood = Instantiate(
                bloodEffectPrefab,
                bloodSpawnPoint.position,
                Quaternion.identity
            );
            Destroy(blood, 0.5f);
        }
    }

    void Die()
    {
        isDead = true;

        // NavMeshAgent'ı durdur
        if (_agent != null)
        {
            _agent.ResetPath();
            _agent.enabled = false;
        }

        gameObject.tag = "Untagged";

        if (enemyShooter != null)
        {
            enemyShooter.SetDead(true);
        }

        // Para ver
        if (currencyScript != null)
        {
            currencyScript.IncreaseBloodMoneyAmount(bloodMoneyAmount);
            Debug.Log($"{bloodMoneyAmount} kan parası eklendi.");
        }
        else
        {
            Currency foundCurrency = FindObjectOfType<Currency>();
            if (foundCurrency != null)
            {
                foundCurrency.IncreaseBloodMoneyAmount(bloodMoneyAmount);
                Debug.Log($"{bloodMoneyAmount} kan parası eklendi (fallback).");
            }
        }

        if (animator != null)
        {
            isDying = true;
            animator.SetBool("isDying", isDying);
            animator.SetBool("isWalking", false);
            Invoke("DestroyEnemy", 2f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    // Public getter - EnemyShooter'ın kullanması için
    public bool IsDead()
    {
        return isDead;
    }

    // NavMeshAgent'a erişim için
    public NavMeshAgent GetNavMeshAgent()
    {
        return _agent;
    }
}
