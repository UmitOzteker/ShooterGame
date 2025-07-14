using UnityEngine;

public class SingleSkill : MonoBehaviour
{
    public GameObject hitVFXPrefab; // Çarpma VFX'i (kan veya glow gibi)
    public float destroyDelay = 0.05f;
    public float Damage = 500f; // Damage büyük harf

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("🔥 mermi yere çarptı ve yok edildi.");
            Destroy(gameObject, destroyDelay);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"🔥 mermi {collision.gameObject.name} düşmana çarptı ve yok edildi.");

            // Hit VFX
            if (hitVFXPrefab != null)
            {
                Instantiate(hitVFXPrefab, collision.contacts[0].point, Quaternion.identity);
            }

            // Damage verme
            FollowCharacter enemy = collision.gameObject.GetComponent<FollowCharacter>();
            if (enemy != null)
            {
                enemy.TakeMeteorDamage(Damage); // Doğru method ve parametre
            }

            Destroy(gameObject, destroyDelay);
        }
    }
}
