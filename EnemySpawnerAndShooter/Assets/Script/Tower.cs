using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private Camera PlayerCamera;

    [SerializeField]
    private GameObject selectedTowerPrefab;
    float nearestDistance = 10000;

    [SerializeField]
    Currency currencyScript;

    void Update()
    {
        if (selectedTowerPrefab == null)
        {
            Debug.LogWarning(
                "Yerleştirilecek kule prefab'ı 'selectedTowerPrefab' değişkenine atanmamış!"
            );
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            // Mevcut para miktarını al
            int currentMoney = 0;

            if (currencyScript != null)
            {
                currentMoney = currencyScript.GetBloodMoneyAmount();
            }
            else
            {
                Currency foundCurrency = FindObjectOfType<Currency>();
                if (foundCurrency != null)
                {
                    currentMoney = foundCurrency.GetBloodMoneyAmount();
                }
            }

            // Para kontrolü
            if (currentMoney < 50)
            {
                Debug.Log("Kule koymak için yeterli para yok! (50 BloodMoney gerekli)");
                return;
            }

            // Para yeterliyse raycast yap
            Ray camRay = PlayerCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(camRay, out hitInfo, 100f))
            {
                Vector3 spawnPosition = hitInfo.point + Vector3.up * 0.5f;
                Instantiate(selectedTowerPrefab, spawnPosition, Quaternion.identity);
                Debug.Log("Kule yerleştirildi: " + spawnPosition);

                // Parayı azalt
                if (currencyScript != null)
                {
                    currencyScript.DecreaseBloodMoneyAmount(50);
                }
                else
                {
                    Currency foundCurrency = FindObjectOfType<Currency>();
                    if (foundCurrency != null)
                    {
                        foundCurrency.DecreaseBloodMoneyAmount(50);
                    }
                }
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
