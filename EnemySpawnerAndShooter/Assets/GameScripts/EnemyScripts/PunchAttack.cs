using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAttack : MonoBehaviour
{
    [Header("Vuruş Ayarları")]
    [SerializeField]
    private float punchDamage = 25f;

    [SerializeField]
    private string playerTag = "Player";

    [Header("Yakınlık Kontrolü")]
    [SerializeField]
    private bool isClosePlayer = false;

    [SerializeField]
    private float detectionRange = 3f;

    [SerializeField]
    private LayerMask playerLayer = -1;

    private bool isAttacking = false;
    private bool canPunch = true;
    private Animator animator;
    private Transform nearestPlayer;

    [Header("Silah Hasar Sistemi")]
    [SerializeField]
    private WeaponHitbox weaponHitbox;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(CheckPlayerProximity());
    }

    void Update()
    {
        // Animator parametresine güncel durumu gönder
        if (animator != null)
        {
            animator.SetBool("isClosePlayer", isClosePlayer);
        }

        // Vuruş tetikleyici
        if (isClosePlayer && canPunch && !isAttacking)
        {
            StartPunch();
        }
    }

    IEnumerator CheckPlayerProximity()
    {
        while (true)
        {
            CheckForNearbyPlayers();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void CheckForNearbyPlayers()
    {
        Collider[] nearbyPlayers = Physics.OverlapSphere(
            transform.position,
            detectionRange,
            playerLayer
        );
        bool playerFound = false;
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (Collider player in nearbyPlayers)
        {

            if (player.gameObject == transform.root.gameObject)
            {
                continue;
            }

            if (player.CompareTag(playerTag))
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player.transform;
                    playerFound = true;
                }
            }
        }

        // Durumu güncelle
        if (playerFound != isClosePlayer)
        {
            isClosePlayer = playerFound;
            nearestPlayer = closestPlayer;

            if (isClosePlayer)
            {
                Debug.Log($"[PunchAttack] Hedef player: {nearestPlayer.name}");
            }
            else
            {
                nearestPlayer = null;
            }
        }
    }

    void StartPunch()
    {

        if (!canPunch || isAttacking)
        {
            return;
        }

        // Oyuncuya dön
        if (nearestPlayer != null)
        {
            Vector3 direction = (nearestPlayer.position - transform.position).normalized;
            direction.y = 0;
            transform.root.rotation = Quaternion.LookRotation(direction);
        }

        // Animasyonu zaten animator bool ile geçiyor
        isAttacking = true;
        canPunch = false;

        StartCoroutine(PunchCooldown());
    }

    IEnumerator PunchCooldown()
    {
        yield return new WaitForSeconds(2f);
        canPunch = true;
    }

    // Animasyon event'leri (vuruş anı)
    public void EnablePunchTrigger()
    {
        isAttacking = true;

        if (weaponHitbox != null)
        {
            weaponHitbox.EnableHit();
        }
    }

    public void DisablePunchTrigger()
    {
        isAttacking = false;

        if (weaponHitbox != null)
        {
            weaponHitbox.DisableHit();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isClosePlayer ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (nearestPlayer != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, nearestPlayer.position);
        }
    }

    // Dışarıya açık ayarlayıcılar
    public void SetDetectionRange(float range)
    {
        detectionRange = range;
    }

    public void SetPunchDamage(float damage)
    {
        punchDamage = damage;

        if (weaponHitbox != null)
        {
            weaponHitbox.SetDamage(damage);
        }
    }
}
