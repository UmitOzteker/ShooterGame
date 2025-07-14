using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireCooldown = 0.5f;
    public float visionDistance = 30f;
    public LayerMask enemyLayerMask;
    public float nextFireTime = 0f;

    void Update()
    {
        GameObject nearestEnemy = EnemyManager.findNearestEnemy(transform.position);
        if (Time.time >= nextFireTime && nearestEnemy != null)
        {
            ShootAt(nearestEnemy.transform.position);
            nextFireTime = Time.time + fireCooldown;
        }
    }

    void ShootAt(Vector3 targetPosition)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();

        if (bulletRig != null)
        {
            Vector3 direction = (targetPosition - firePoint.position).normalized;
            float speed = bulletScript != null ? bulletScript.speed : 50f; // Bullet prefab Inspector'dan speed
            bulletRig.AddForce(direction * speed, ForceMode.Impulse);
        }
        Destroy(bulletObj, 5f);
    }
}
