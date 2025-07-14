using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    [SerializeField]
    private float damage = 25f;

    [SerializeField]
    private string playerTag = "Player";

    private bool canHit = false;
    private List<GameObject> hitTargets = new();
    private Transform weaponOwner;

    void Start()
    {
        weaponOwner = transform.root;

        // Collider kontrolü
        Collider hitboxCollider = GetComponent<Collider>();
    }

    public void EnableHit()
    {
        canHit = true;
        hitTargets.Clear();
    }

    public void DisableHit()
    {
        canHit = false;
        hitTargets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!canHit)
        {
            return;
        }

        // Kendi objesine zarar verme kontrolü
        if (other.transform.root == weaponOwner)
        {
            return;
        }

        // Daha önce vurulmuş mı kontrolü
        if (hitTargets.Contains(other.gameObject))
        {
            return;
        }

        // Tag kontrolü
        if (!other.CompareTag(playerTag))
        {
            return;
        }


        // PlayerHealth arama - önce kendi objesinde
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            playerHealth = other.GetComponentInParent<PlayerHealth>();
        }

        if (playerHealth != null)
        {

            // Hasar ver
            playerHealth.TakeDamage(damage);

            // Hedefi listeye ekle
            hitTargets.Add(other.gameObject);

            // Tek seferlik hasar için deaktif et
            canHit = false;

        }
        else
        {
 
            Transform current = other.transform;
            while (current != null)
            {
                current = current.parent;
            }
        }
    }

    // Ek debug metodu
    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[WeaponHitbox] 🚪 TRIGGER EXIT: {other.name} (Tag: {other.tag})");
    }

    // Sürekli debug bilgisi
    void Update()
    {
        if (canHit)
        {

            // Manuel mesafe kontrolü
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);


                // Eğer 2 birim içindeyse manuel hasar ver
                if (distance < 2f)
                {
                    PlayerHealth ph = player.GetComponent<PlayerHealth>();
                    if (ph != null)
                    {
                        ph.TakeDamage(damage);
                        canHit = false;
                        break;
                    }
                }
            }
        }
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;

    }
}
