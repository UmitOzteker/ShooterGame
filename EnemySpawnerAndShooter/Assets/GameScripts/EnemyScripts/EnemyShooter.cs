using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Ateş Etme Ayarları")]
    [SerializeField]
    GameObject enemyBulletPrefab;

    [SerializeField]
    Transform firePoint;

    [SerializeField]
    float fireCooldown = 1f;

    [SerializeField]
    float shootingRange = 20f;

    [SerializeField]
    float minShootingDistance = 3f; // Bu mesafeden daha yakınsa ateş etmez

    [SerializeField]
    LayerMask playerLayerMask = -1;

    [SerializeField]
    LayerMask obstacleLayerMask = -1; // Engelleri kontrol etmek için

    [Header("Referanslar")]
    [SerializeField]
    GameObject player;

    private float nextFireTime = 0f;
    private bool isDead = false;
    private FollowCharacter followScript;

    void Awake()
    {
        followScript = GetComponent<FollowCharacter>();
    }

    void Start()
    {
        FindPlayer();

        if (firePoint == null)
        {
            // firePoint yoksa düşmanın önünde bir nokta oluştur
            GameObject firePointObj = new GameObject("FirePoint");
            firePointObj.transform.SetParent(transform);
            firePointObj.transform.localPosition = Vector3.forward * 1f;
            firePoint = firePointObj.transform;
        }
    }

    void FindPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        // Düşman ölüyse ateş etme
        if (isDead || player == null)
            return;

        // FollowCharacter script'inden ölüm durumunu kontrol et
        if (followScript != null && followScript.GetComponent<FollowCharacter>())
        {
            // FollowCharacter'dan isDead durumunu almanın bir yolu olmalı
            // Şimdilik basit bir kontrol yapalım
        }

        // Oyuncuya ateş etme mantığı
        if (Time.time >= nextFireTime && CanShootAtPlayer())
        {
            ShootAtPlayer();
            nextFireTime = Time.time + fireCooldown;
        }
    }

    bool CanShootAtPlayer()
    {
        if (player == null)
            return false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Mesafe kontrolleri
        if (distanceToPlayer > shootingRange)
        {
            return false; // Çok uzak
        }

        if (distanceToPlayer < minShootingDistance)
        {
            return false; // Çok yakın
        }

        // Görüş açısı kontrolü (düşman oyuncuya bakıyor mu?)
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle > 60f) // 60 derece görüş açısı
        {
            return false; // Görüş açısı dışında
        }

        // Engel kontrolü (Raycast)
        RaycastHit hit;
        Vector3 rayStart = firePoint.position;
        Vector3 rayDirection = (player.transform.position - rayStart).normalized;

        if (Physics.Raycast(rayStart, rayDirection, out hit, distanceToPlayer, obstacleLayerMask))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                return false; // Arada engel var
            }
        }

        return true;
    }

    void ShootAtPlayer()
    {
        if (enemyBulletPrefab == null || firePoint == null)
        {
            return;
        }

        // Mermi oluştur
        GameObject bulletObj = Instantiate(
            enemyBulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        // Mermi tag'ini ayarla
        bulletObj.tag = "EnemyBullet";

        // Mermi script'ini al
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();

        if (bulletRig != null)
        {
            // Oyuncuya doğru yön hesapla
            Vector3 direction = (player.transform.position - firePoint.position).normalized;
            float speed = bulletScript != null ? bulletScript.speed : 30f;

            // Mermiyi fırlat
            bulletRig.AddForce(direction * speed, ForceMode.Impulse);
        }

        // Mermiyi 5 saniye sonra yok et
        Destroy(bulletObj, 5f);
    }

    // Düşman öldüğünde çağrılacak metod
    public void SetDead(bool dead)
    {
        isDead = dead;
    }

    // Debug için görselleştirme
    void OnDrawGizmosSelected()
    {
        if (firePoint != null)
        {
            // Ateş menzilini göster
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shootingRange);

            // Minimum mesafeyi göster
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, minShootingDistance);

            // Ateş noktasını göster
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(firePoint.position, 0.2f);

            // Görüş açısını göster
            Gizmos.color = Color.green;
            Vector3 forward = transform.forward * shootingRange;
            Vector3 right = Quaternion.Euler(0, 60, 0) * forward;
            Vector3 left = Quaternion.Euler(0, -60, 0) * forward;

            Gizmos.DrawRay(transform.position, forward);
            Gizmos.DrawRay(transform.position, right);
            Gizmos.DrawRay(transform.position, left);
        }
    }
}
