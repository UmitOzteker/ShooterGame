using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Camera PlayerCamera; // Oyuncu kamerasını Inspector'dan sürükleyip bırakın
    [SerializeField] private GameObject selectedTowerPrefab; // Yerleştirilecek kule prefab'ı, bunu Inspector'dan atayın
    
    // Enemy targeting variables
    public GameObject NearestEnemy;
    float nearestDistance = 10000;
    
    // Not: CurrentPlacingTower değişkeni, eğer kuleyi seçip anında bir "hayalet" (ghost) nesne gibi
    // fareyle gezdirmek isterseniz kullanışlıdır. Basit yerleştirme için gerek yok.
    // private GameObject CurrentPlacingTower;

    void Update()
    {
        // Eğer yerleştirilecek bir kule prefab'ı atanmamışsa, uyarı ver ve çık
        if (selectedTowerPrefab == null)
        {
            Debug.LogWarning("Yerleştirilecek kule prefab'ı 'selectedTowerPrefab' değişkenine atanmamış!");
            return; // Update döngüsünden çık
        }

        // Sağ tık algıla
        if (Input.GetMouseButtonDown(1))
        {
            // Kamera konumundan farenin olduğu noktaya bir ışın (ray) oluştur
            Ray camRay = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo; // Işının çarptığı yerin bilgilerini tutacak değişken

            // Işın bir şeye çarptıysa (örneğin zemin)
            if (Physics.Raycast(camRay, out hitInfo, 100f))
            {
                // Y pozisyonunu biraz yukarı kaydır ki kule yerin içinde doğmasın
                Vector3 spawnPosition = hitInfo.point + Vector3.up * 0.5f; // 0.5 birim yukarı
                
                // Sağ tıklandığında, fare imlecinin baktığı yere kuleyi oluştur
                // Quaternion.identity, kulenin dönme açısını sıfır (orijinal) olarak ayarlar
                Instantiate(selectedTowerPrefab, spawnPosition, Quaternion.identity);
                Debug.Log("Kule yerleştirildi: " + spawnPosition);
            }
            else
            {
                Debug.Log("Işın 100 birim içinde hiçbir şeye çarpmadı.");
            }
        }
    }

    public void SetTowerToPlace(GameObject towerPrefabToSet)
    {
        selectedTowerPrefab = towerPrefabToSet;
        Debug.Log(towerPrefabToSet.name + " kulesi yerleştirilmek üzere seçildi.");
    }
}