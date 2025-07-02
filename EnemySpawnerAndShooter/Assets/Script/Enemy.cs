using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    public float speed = 1f;

    [SerializeField]
    float currentHealth = 100f;

    [SerializeField]
    float maxHealth = 100f;

    [SerializeField]
    HealthBar healthBar;

    [SerializeField]
    float minDistance = 2f;

    [SerializeField]
    float maxDistance = 5f;

    [SerializeField]
    float stopDistance = 2f;

    [SerializeField]
    Currency currencyScript;

    // Ölüm durumunu kontrol etmek için
    private bool isDead = false;

    void Start()
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

            if (currencyScript == null)
            {
                Debug.LogError("Currency script hiçbir GameObject'te bulunamadı!");
            }
            else
            {
                Debug.Log($"Currency script bulundu: {currencyScript.gameObject.name}");
            }
        }

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
        Debug.Log($"Başlangıç canı: {currentHealth}/{maxHealth}");
    }

    void Update()
    {
        if (player == null)
            return;

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
            transform.position = Vector3.MoveTowards(
                transform.position,
                transform.position + direction,
                currentSpeed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead)
            return;

        if (other.CompareTag("Bullet"))
        {
            currentHealth -= 50f;

            if (healthBar != null)
            {
                healthBar.UpdateHealthBar(currentHealth, maxHealth);
            }

            Destroy(other.gameObject);

            if (currentHealth <= 0 && !isDead)
            {
                isDead = true;
                if (currencyScript != null)
                {
                    currencyScript.IncreaseBloodMoneyAmount(50);
                }
                else
                {
                    Currency foundCurrency = FindObjectOfType<Currency>();
                    if (foundCurrency != null)
                    {
                        foundCurrency.IncreaseBloodMoneyAmount(50);
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}
