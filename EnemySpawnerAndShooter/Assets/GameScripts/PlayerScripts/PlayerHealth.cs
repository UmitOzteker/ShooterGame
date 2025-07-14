using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Oyuncu Verileri")]
    [SerializeField]
    PlayerData PlayerData; // PlayerData olarak kullanılacak

    [Header("UI Referansları")]
    [SerializeField]
    Image HealthBar;

    [SerializeField]
    HealthBar healthBarScript; // HealthBar script referansı (eğer varsa)

    [Header("Game Over Settings")]
    [SerializeField]
    string gameOverScreenName = "GameOverScreen"; // GameOverScreen obje adı

    [SerializeField]
    bool pauseGameOnDeath = true; // Oyunu durdur

    [SerializeField]
    float gameOverDelay = 2f; // Game Over'dan önce bekleme süresi,

    [Header("Kan Efekti")]
    [SerializeField]
    private GameObject bloodEffectPrefab;

    [SerializeField]
    private Transform bloodSpawnPoint; // Efektin çıkacağı yer

    private float currentHealth;
    private float maxHealth;
    private bool isDead = false;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // PlayerSpawner tarafından InitializePlayer çağrılmadıysa normal başlat
        if (PlayerData != null && currentHealth == 0)
        {
            InitializeFromPlayerData();
            FindReferences();
            UpdateHealthBar();
            if (HealthBar == null)
            {
                GameObject healthBarObj = GameObject.Find("HealthBarObject");
                if (healthBarObj != null)
                {
                    HealthBar = healthBarObj.GetComponentInChildren<Image>();
                }
            }
        }
    }

    // PlayerSpawner tarafından çağrılacak initialize metodu
    public void InitializePlayer(
        PlayerData playerData,
        Image healthBarImage,
        HealthBar healthBarScript = null
    )
    {
        PlayerData = playerData;
        HealthBar = healthBarImage;
        this.healthBarScript = healthBarScript;

        if (PlayerData != null)
        {
            InitializeFromPlayerData();
        }
        else
        {
            SetDefaultValues();
        }

        FindReferences();
        UpdateHealthBar();
    }

    void InitializeFromPlayerData()
    {
        maxHealth = PlayerData.maxHealth;
        currentHealth = maxHealth;
    }

    void SetDefaultValues()
    {
        maxHealth = 100f;
        currentHealth = 100f;
    }

    void FindReferences()
    {
        GameObject healthBarObj = GameObject.Find("PlayerHealthBarObject");

        if (healthBarObj != null)
        {
            // Doğrudan PlayerHealthBar alt objesini bul ve Image'ını al
            Transform fillTransform = healthBarObj.transform.Find("PlayerHealthBar");
            if (fillTransform != null)
            {
                HealthBar = fillTransform.GetComponent<Image>();
            }

            // HealthBar script ataması
            if (healthBarScript == null)
            {
                healthBarScript = healthBarObj.GetComponent<HealthBar>();
            }
        }
    }

    void UpdateHealthBar()
    {
        float fill = currentHealth / maxHealth;

        if (healthBarScript != null)
        {
            healthBarScript.UpdateHealthBar(currentHealth, maxHealth);
        }
        if (HealthBar != null)
        {
            HealthBar.fillAmount = fill;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (bloodEffectPrefab != null && bloodSpawnPoint != null)
        {
            GameObject blood = Instantiate(
                bloodEffectPrefab,
                bloodSpawnPoint.position,
                Quaternion.identity
            );

            Destroy(blood, 0.5f);
        }

        UpdateHealthBar();

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        if (isDead)
            return;

        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead)
            return;

        if (other.CompareTag("EnemyBullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            TakeDamage(50f);
        }
    }

    bool isDying = false;

    void Die()
    {
        isDead = true;
        gameObject.tag = "Untagged";
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        // Game Over Screen'i aç

        if (animator != null)
        {
            isDying = true;
            animator.SetBool("isDying", isDying);
            Invoke("DestroyPlayer", 3f);
        }
        else
        {
            ShowGameOverScreen();
        }
        // Oyuncuyu devre dışı bırak (GameOverScreen açıldıktan sonra)
        StartCoroutine(DisablePlayerAfterDelay());
    }

    void DestroyPlayer()
    {
        ShowGameOverScreen();
    }

    void ShowGameOverScreen()
    {
        // GameOverScreen'i sahne içinde ara (alt objeler dahil)
        GameObject gameOverScreen = FindGameOverScreen();

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);

            // Oyunu durdur (opsiyonel)
            if (pauseGameOnDeath)
            {
                Time.timeScale = 0f;
            }

            // Cursor'u aktif et (opsiyonel)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // GameOverScreen'i sahne içinde bulma metodu
    GameObject FindGameOverScreen()
    {
        // Önce direkt GameObject.Find dene
        GameObject gameOverScreen = GameObject.Find(gameOverScreenName);
        if (gameOverScreen != null)
        {
            return gameOverScreen;
        }

        // Bulunamazsa, Canvas altında ara
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            Transform found = canvas.transform.Find(gameOverScreenName);
            if (found != null)
            {
                return found.gameObject;
            }
        }

        // Hala bulunamazsa, tüm GameOverScreen isimli objeleri ara
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == gameOverScreenName)
            {
                return obj;
            }
        }

        return null;
    }

    // Static metodlar - referans almadan kullanılabilir
    public static void LoadMainMenuStatic()
    {
        // Time.timeScale'i normale çevir
        Time.timeScale = 1f;

        SceneManager.LoadSceneAsync(0);
    }

    public static void RestartGameStatic()
    {
        // Time.timeScale'i normale çevir
        Time.timeScale = 1f;

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public static void QuitGameStatic()
    {
        Debug.Log("Oyundan çıkılıyor...");
        Application.Quit();
    }

    // Ana menüye git
    public void LoadMainMenu()
    {
        LoadMainMenuStatic();
    }

    // Oyunu yeniden başlat
    public void RestartGame()
    {
        RestartGameStatic();
    }

    // Oyundan çık
    public void QuitGame()
    {
        QuitGameStatic();
    }

    IEnumerator DisablePlayerAfterDelay()
    {
        // Belirtilen süre sonra oyuncuyu devre dışı bırak
        yield return new WaitForSeconds(gameOverDelay);
        gameObject.SetActive(false);
    }

    // Public metod - dışarıdan GameOverScreen açmak için
    public void ForceGameOver()
    {
        if (!isDead)
        {
            Die();
        }
    }

    // Public getter'lar
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    // GameOverScreen obje ismini dışarıdan set etmek için
    public void SetGameOverScreenName(string screenName)
    {
        gameOverScreenName = screenName;
    }

    // GameOverScreen'i manuel olarak açmak için
    public void OpenGameOverScreen()
    {
        ShowGameOverScreen();
    }
}
