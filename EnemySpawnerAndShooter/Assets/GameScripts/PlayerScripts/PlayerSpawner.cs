using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject plane;

    [Header("Player Health Settings")]
    [SerializeField]
    PlayerData PlayerData; // Player için sağlık verilerini içeren ScriptableObject

    [SerializeField]
    Image playerHealthBarImage; // Player'ın health bar'ı için Image reference

    [SerializeField]
    HealthBar playerHealthBarScript; // Eğer HealthBar script'i kullanıyorsanız

    private GameObject currentPlayer; // Spawn edilen oyuncu referansı

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (CharacterDisplayManager.selectedCharacterData != null && plane != null)
        {
            Vector3 center = plane.GetComponent<Renderer>().bounds.center;

            GameObject prefab = CharacterDisplayManager.selectedCharacterData.prefab;
            float offsetY = 0f;
            CapsuleCollider col = prefab.GetComponent<CapsuleCollider>();
            if (col != null)
            {
                offsetY = -col.center.y + col.height / 2f;
            }
            else
            {
                BoxCollider box = prefab.GetComponent<BoxCollider>();
                if (box != null)
                {
                    offsetY = -box.center.y + box.size.y / 2f;
                }
            }
            float y = plane.GetComponent<Renderer>().bounds.min.y + offsetY;
            Vector3 spawnPos = new Vector3(center.x, y, center.z);

            // Oyuncuyu spawn et
            currentPlayer = Instantiate(prefab, spawnPos, Quaternion.identity);
            currentPlayer.tag = "Player";

            // PlayerHealth component'ini ekle ve ayarla
            SetupPlayerHealth();
        }
    }

    void SetupPlayerHealth()
    {
        if (currentPlayer != null)
        {
            // PlayerHealth component'ini kontrol et, yoksa ekle
            PlayerHealth playerHealth = currentPlayer.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                playerHealth = currentPlayer.AddComponent<PlayerHealth>();
            }

            // PlayerHealth'i başlat
            playerHealth.InitializePlayer(PlayerData, playerHealthBarImage, playerHealthBarScript);
        }
    }

    // Public metodlar - diğer script'lerin kullanması için
    public GameObject GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public PlayerHealth GetPlayerHealth()
    {
        if (currentPlayer != null)
        {
            return currentPlayer.GetComponent<PlayerHealth>();
        }
        return null;
    }

    public void RespawnPlayer()
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }
        SpawnPlayer();
    }
}
