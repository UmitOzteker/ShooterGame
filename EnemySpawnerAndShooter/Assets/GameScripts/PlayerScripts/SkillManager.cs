using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    None,
    SingleTarget,
    Area,
}

public class SkillManager : MonoBehaviour
{
    [Header("UI Butonları")]
    [SerializeField]
    private Button skill1Button; // Fireball butonu

    [SerializeField]
    private Button skill2Button; // Meteor butonu

    [SerializeField]
    private Image skill1Icon; // Fireball ikonu

    [SerializeField]
    private Image skill2Icon; // Meteor ikonu

    [SerializeField]
    private Color selectedColor = Color.white; // Seçili renk

    [SerializeField]
    private Color normalColor = Color.gray; // Normal renk

    [SerializeField]
    private Color cooldownColor = Color.red; // Cooldown rengi

    [Header("Skill Prefabları")]
    [SerializeField]
    private GameObject singleTargetSkillPrefab;

    [SerializeField]
    private GameObject aoeSkillPrefab;

    [Header("Skill Ayarları")]
    [SerializeField]
    private float singleTargetCooldown = 3f;

    [SerializeField]
    private float aoeCooldown = 5f;

    [Header("Kontrol Tuşları")]
    [SerializeField]
    private KeyCode skillCastKey = KeyCode.Space; // Boşluk tuşu

    [Header("Spawn Noktası")]
    [SerializeField]
    private Transform skillSpawnPoint;

    [Header("Katmanlar")]
    [SerializeField]
    private LayerMask groundLayer;

    private SkillType selectedSkill = SkillType.None;
    private bool singleTargetReady = true;
    private bool aoeReady = true;

    // Cooldown zamanlarını takip et
    private float singleTargetCooldownTimer = 0f;
    private float aoeCooldownTimer = 0f;

    void Start()
    {
        Debug.Log("=== SkillManager Başlatıldı ===");

        // Butonlara event listener ekle
        if (skill1Button != null)
        {
            skill1Button.onClick.AddListener(SelectSingleTargetSkill);
            Debug.Log(">> Skill1 Button bağlandı");
        }
        else
        {
            Debug.LogWarning(">> Skill1 Button atanmamış!");
        }

        if (skill2Button != null)
        {
            skill2Button.onClick.AddListener(SelectAOESkill);
            Debug.Log(">> Skill2 Button bağlandı");
        }
        else
        {
            Debug.LogWarning(">> Skill2 Button atanmamış!");
        }

        Debug.Log(">> UI ilk kez güncellendi");
    }

    void Update()
    {
        UpdateCooldowns();

        if (Input.GetKeyDown(skillCastKey))
        {
            Debug.Log("=== Space Tuşu Basıldı ===");
            Debug.Log($">> Seçili Skill: {selectedSkill}");

            if (selectedSkill == SkillType.SingleTarget && singleTargetReady)
            {
                Debug.Log(">> 🔥 Tek hedef skill kullanılıyor...");
                TryCastSingleTarget();
            }
            else if (selectedSkill == SkillType.Area && aoeReady)
            {
                Debug.Log(">> ☄️ AOE skill kullanılıyor...");
                TryCastAOE();
            }
            else if (selectedSkill == SkillType.None)
            {
                Debug.Log(">> ❌ Önce bir skill seçin! (Butona tıklayın)");
            }
            else
            {
                Debug.Log(">> ❌ Seçili skill cooldown'da veya hazır değil!");
            }
        }
    }

    private void UpdateCooldowns()
    {
        // Single Target Cooldown
        if (!singleTargetReady)
        {
            singleTargetCooldownTimer -= Time.deltaTime;
            if (singleTargetCooldownTimer <= 0f)
            {
                singleTargetReady = true;
                Debug.Log(">> Tek hedef skill tekrar hazır!");
            }
        }

        if (!aoeReady)
        {
            aoeCooldownTimer -= Time.deltaTime;
            if (aoeCooldownTimer <= 0f)
            {
                aoeReady = true;
                Debug.Log(">> AOE skill tekrar hazır!");
            }
        }
    }

    public void SelectSingleTargetSkill()
    {
        Debug.Log("=== SelectSingleTargetSkill Çağrıldı ===");

        if (singleTargetReady)
        {
            selectedSkill = SkillType.SingleTarget;
            Debug.Log(">> ✅ Tek hedef skill seçildi. Space tuşu ile kullanın.");
        }
        else
        {
            Debug.Log(
                $">> ❌ Tek hedef skill cooldown'da! {singleTargetCooldownTimer:F1} saniye kaldı."
            );
        }
    }

    public void SelectAOESkill()
    {
        Debug.Log("=== SelectAOESkill Çağrıldı ===");

        if (aoeReady)
        {
            selectedSkill = SkillType.Area;
            Debug.Log(">> ✅ Alan etkili skill seçildi. Space tuşu ile kullanın.");
        }
        else
        {
            Debug.Log($">> ❌ AOE skill cooldown'da! {aoeCooldownTimer:F1} saniye kaldı.");
        }
    }

    // === Skill Durumlarını Kontrol Etme ===
    public bool IsSingleTargetReady()
    {
        return singleTargetReady;
    }

    public bool IsAOEReady()
    {
        return aoeReady;
    }

    public float GetSingleTargetCooldownTime()
    {
        return singleTargetCooldownTimer;
    }

    public float GetAOECooldownTime()
    {
        return aoeCooldownTimer;
    }

   private void TryCastSingleTarget()
{
    Debug.Log("=== TryCastSingleTarget Başladı ===");

    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    Debug.Log($">> Raycast başlatıldı. Fare pozisyonu: {Input.mousePosition}");

    if (Physics.Raycast(ray, out RaycastHit hit))
    {
        Debug.Log($">> Raycast çarptı: {hit.collider.name} (Tag: {hit.collider.tag})");

        if (hit.collider.CompareTag("Enemy"))
        {
            GameObject enemy = hit.collider.gameObject;
            Debug.Log($">> ✅ Hedef bulundu: {enemy.name}");
            Debug.Log($">> 🔥 Tek hedef skill {enemy.name} üzerine uygulanıyor...");

            // Yukarıdan düşecek şekilde spawn et
            if (singleTargetSkillPrefab != null)
            {
                Vector3 spawnAbove = enemy.transform.position + Vector3.up * 10f; // 10 birim yukarıdan
                GameObject fireball = Instantiate(singleTargetSkillPrefab, spawnAbove, Quaternion.identity);

                // Rigidbody varsa gravity ile düşsün
                Rigidbody rb = fireball.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = true;
                }
                else
                {
                    Debug.LogWarning(">> Fireball prefabında Rigidbody yok, ekleyin ki düşebilsin.");
                }

                Debug.Log($">> Fireball yukarıdan spawn edildi: {spawnAbove}");
            }
            else
            {
                Debug.LogWarning(">> ❌ Skill prefab atanmadı!");
            }

            // Cooldown başlat
            singleTargetReady = false;
            singleTargetCooldownTimer = singleTargetCooldown;
            Debug.Log($">> Cooldown başladı: {singleTargetCooldown} saniye");

            selectedSkill = SkillType.None;
            Debug.Log(">> Skill seçimi sıfırlandı");
        }
        else
        {
            Debug.Log($">> ❌ Hedef Enemy değil! Tag: {hit.collider.tag}");
        }
    }
    else
    {
        Debug.Log(">> ❌ Raycast hiçbir şeye çarpmadı!");
    }
}


    private void TryCastAOE()
    {
        Debug.Log("=== TryCastAOE Başladı ===");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log($">> Raycast başlatıldı. Fare pozisyonu: {Input.mousePosition}");
        Debug.Log($">> Ground Layer değeri: {groundLayer}");

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Debug.Log(
                $">> Çarpan obje: {hit.collider.name}, Layer: {hit.collider.gameObject.layer}"
            );

            Vector3 targetPos = hit.point;
            Debug.Log($">> ✅ Hedef pozisyon: {targetPos}");
            Debug.Log($">> ☄️ Meteor alan skilli pozisyona düştü: {targetPos}");

            if (aoeSkillPrefab != null)
            {
                Vector3 spawnAbove = targetPos + Vector3.up * 10f;
                GameObject meteor = Instantiate(aoeSkillPrefab, spawnAbove, Quaternion.identity);
                Debug.Log($">> Meteor oluşturuldu: {spawnAbove}");
            }
            else
            {
                Debug.LogWarning(">> ❌ AOE Skill prefab atanmamış!");
            }

            aoeReady = false;
            aoeCooldownTimer = aoeCooldown;
            Debug.Log($">> AOE Cooldown başladı: {aoeCooldown} saniye");

            selectedSkill = SkillType.None;
            Debug.Log(">> Skill seçimi sıfırlandı");
        }
        else
        {
            Debug.Log(">> ❌ Hiçbir objeye çarpmadı!");
        }
    }
}
