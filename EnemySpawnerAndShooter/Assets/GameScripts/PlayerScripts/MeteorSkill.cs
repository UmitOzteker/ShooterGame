using UnityEngine;

public class MeteorSkill : MonoBehaviour
{
    [Header("Hasar Ayarları")]
    public float damage = 60f;
    public float explosionRadius = 5f;

    [Header("Efektler")]
    public GameObject explosionEffect;

    [Header("Gecikme")]
    public float delayBeforeImpact = 1.5f;

    void Start()
    {
        Debug.Log("=== MeteorSkill Başlatıldı ===");
        Debug.Log($">> Hasar: {damage}, Radius: {explosionRadius}, Gecikme: {delayBeforeImpact}s");

        // Gecikmeli patlama efekti
        Invoke(nameof(ShowExplosionEffect), delayBeforeImpact);
    }

    void ShowExplosionEffect()
    {
        Debug.Log("=== Meteor Patlama Efekti ===");
        Debug.Log($">> Patlama pozisyonu: {transform.position}");

        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(
                explosionEffect,
                transform.position,
                Quaternion.identity
            );
            Debug.Log(">> Patlama efekti oluşturuldu");

            Destroy(effect, 3f);
        }
        else
        {
            Debug.LogWarning(">> Patlama efekti atanmamış!");
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        int enemyCount = 0;
        int damagedEnemies = 0;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                enemyCount++;

                FollowCharacter enemy = hit.GetComponent<FollowCharacter>();
                if (enemy != null)
                {
                    enemy.SendMessage(
                        "TakeMeteorDamage",
                        damage,
                        SendMessageOptions.DontRequireReceiver
                    );
                    damagedEnemies++;
                    Debug.Log($">> {hit.name} düşmanına {damage} hasar verildi!");
                }
                else
                {
                    Debug.LogWarning($">> {hit.name} düşmanında FollowCharacter bulunamadı!");
                }
            }
        }

        Debug.Log(
            $">> Patlama alanında {enemyCount} düşman bulundu, {damagedEnemies} düşmana hasar verildi"
        );

        Destroy(gameObject);
        Debug.Log(">> Meteor yok edildi");
    }
}
