using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticShooter : MonoBehaviour
{
    // Değişken isimleri daha anlaşılır hale getirildi.
    [Header("Mermi ve Atış Ayarları")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 50f;
    public float fireCooldown = 0.5f;
    public float visionDistance = 30f;
    public LayerMask enemyLayerMask;

    private float nextFireTime = 0f;

    void Update()
    {
        GameObject nearestEnemy = EnemyManager.findNearestEnemy(transform.position);

        // Eğer hedef varsa ve ateş zamanı geldiyse
        if (Time.time >= nextFireTime && nearestEnemy != null)
        {
            ShootAt(nearestEnemy.transform.position);
            nextFireTime = Time.time + fireCooldown;
        }
    }

    void ShootAt(Vector3 targetPosition)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();

        if (bulletRig != null)
        {
            Vector3 direction = (targetPosition - firePoint.position).normalized;
            bulletRig.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        }

        Destroy(bulletObj, 5f);
    }
}
