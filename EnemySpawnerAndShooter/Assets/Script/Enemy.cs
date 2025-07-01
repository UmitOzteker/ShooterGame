// FollowCharacter.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    [SerializeField] GameObject player;
    public float speed = 1f;
    [SerializeField] float currentHealth = 100f;
    [SerializeField] float maxHealth = 100f;

    [SerializeField] HealthBar healthBar;
    [SerializeField] float minDistance = 2f;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] float stopDistance = 2f;

    void Start()
    {
        // Player bul
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        // HealthBar bul
        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<HealthBar>();
        }

        // Health bar'ı başlat
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
            Debug.Log("HealthBar başlatıldı");
        }
        else
        {
            Debug.LogError("HealthBar bulunamadı!");
        }

        Debug.Log($"Başlangıç canı: {currentHealth}/{maxHealth}");
    }

    void Update()
    {
        if (player == null) return;

        Vector3 playerPos = player.transform.position;
        Vector3 playerPosFlat = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        float distance = Vector3.Distance(transform.position, playerPosFlat);

        // Oyuncuya bak
        Vector3 lookDirection = (playerPosFlat - transform.position).normalized;
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = targetRotation;
        }

        // Hareket
        if (distance > stopDistance)
        {
            float currentSpeed = distance > maxDistance ? speed * 2f : speed;
            Vector3 direction = (playerPosFlat - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, currentSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger girdi: {other.name}, Tag: {other.tag}");
        
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Bullet ile çarpıştı!");
            
            currentHealth -= 50f;
            Debug.Log($"Can azaldı: {currentHealth}/{maxHealth}");

            if (healthBar != null)
            {
                healthBar.UpdateHealthBar(currentHealth, maxHealth);
                Debug.Log("HealthBar güncellendi");
            }
            else
            {
                Debug.LogError("HealthBar null!");
            }

            Destroy(other.gameObject);

            if (currentHealth <= 0)
            {
                Debug.Log("Düşman öldü!");
                Destroy(gameObject);
            }
        }
    }

    // Test için public method
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
        
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}