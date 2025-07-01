using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticShooter : MonoBehaviour
{
    // Değişken isimleri daha anlaşılır hale getirildi.
    [Header("Mermi ve Atış Ayarları")]
    public GameObject bulletPrefab;      // Daha standart bir isimlendirme: "Bullet" yerine "bulletPrefab".
    public Transform firePoint;          // "spawnpoint" yerine daha yaygın olan "firePoint" kullanıldı.
    public float bulletSpeed = 50f;      // Merminin hızı için kendi değişkeni. "enemySpeed" değil.
    public float fireCooldown = 0.5f;
    public float visionDistance = 30f;          // Görüş mesafesi
    public LayerMask enemyLayerMask;     // İki atış arasındaki saniye cinsinden bekleme süresi (0.5 saniye).

    private float nextFireTime = 0f;     // Bir sonraki ateşin ne zaman yapılabileceğini tutan zamanlayıcı.

    // Update, her frame'de çağrılır.
    void Update()
    {
        // Oyunun başlangıcından beri geçen süre, bir sonraki ateş zamanından büyük veya eşit mi?
        if (Time.time >= nextFireTime && CanSeeEnemy())
        {
            Shoot(); // Ateş etme fonksiyonunu çağır.
            nextFireTime = Time.time + fireCooldown; // Bir sonraki ateş zamanını geleceğe ayarla.
        }
    }


    bool CanSeeEnemy()
    {
        RaycastHit hit;
        Vector3 direction = firePoint.forward;

        Debug.DrawRay(firePoint.position, direction * visionDistance, Color.red);

        if (Physics.Raycast(firePoint.position, direction, out hit, visionDistance, enemyLayerMask))
        {
            if (this.gameObject.tag != "Player")
            {
                Debug.Log($"Raycast çarptı: {hit.collider.name}, Tag: {hit.collider.tag}");
            }

            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Düşman görüldü!");
                return true;
            }
        }
        else
        {
            Debug.Log("Raycast hiçbir şeye çarpmadı.");
        }
        return false;
    }


    void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();

        if (bulletRig != null)
        {
            bulletRig.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);
        }

        // Bu satırınız zaten doğruydu. Mermiyi 5 saniye sonra yok eder.
        Destroy(bulletObj, 5f);
    }
}

