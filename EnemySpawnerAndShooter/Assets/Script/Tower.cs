using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private TowerData selectedTowerData;

    [SerializeField]
    private Currency currencyScript;

    private Vector3 clickedPosition;
    private Vector2 clickedScreenPosition;

    void Update()
    {
        if (selectedTowerData == null)
        {
            Debug.LogWarning("Yerleştirilecek kule prefab atanmadı.");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Sol tıklama algılandı.");

            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Sol tık bir UI öğesi üzerinde yapıldı, popup açılmayacak.");
                return;
            }

            if (currencyScript == null)
            {
                currencyScript = FindObjectOfType<Currency>();
                if (currencyScript == null)
                {
                    Debug.LogError("Currency script sahnede bulunamadı!");
                    return;
                }
            }

            Ray camRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(playerCamera.transform.position, camRay.direction * 100f, Color.red, 2f);

            if (Physics.Raycast(camRay, out RaycastHit hitInfo, 100f))
            {
                clickedPosition = hitInfo.point;
                clickedScreenPosition = Input.mousePosition;

                Debug.Log("Raycast başarılı.");
                Debug.Log($"Tıklanan dünya pozisyonu: {clickedPosition}");
                Debug.Log($"Tıklanan ekran pozisyonu: {clickedScreenPosition}");

                if (selectedTowerData != null)
                {
                    Debug.Log(
                        $"Seçilen kule: {selectedTowerData.towerName} | Maliyet: {selectedTowerData.cost}"
                    );
                }

                if (PopupManager.Instance != null)
                {
                    Debug.Log("PopupManager bulundu. Popup açılıyor...");
                    PopupManager.Instance.OpenTowerPopup(clickedScreenPosition);
                }
                else
                {
                    Debug.LogError("PopupManager.Instance bulunamadı. Popup açılamadı.");
                }
            }
            else
            {
                Debug.LogWarning("Raycast hiçbir objeye çarpmadı.");
            }
        }
    }

    public void PlaceTower()
    {
        Debug.Log("PlaceTower() fonksiyonu çağrıldı.");
        int currentMoney = currencyScript.GetBloodMoneyAmount();
        Debug.Log($"Mevcut para: {currentMoney}, Gerekli: {selectedTowerData.cost}");

        if (currentMoney < selectedTowerData.cost)
        {
            Debug.LogWarning(
                $"Yetersiz para. Gerekli: {selectedTowerData.cost}, Mevcut: {currentMoney}"
            );
            return;
        }

        Vector3 spawnPosition = clickedPosition + Vector3.up * 0.5f;
        Debug.Log(
            $"Kule Instantiate ediliyor. Pozisyon: {spawnPosition}, Prefab: {selectedTowerData.prefab.name}"
        );

        Instantiate(selectedTowerData.prefab, spawnPosition, Quaternion.identity);
        currencyScript.DecreaseBloodMoneyAmount(selectedTowerData.cost);

        Debug.Log("Kule başarıyla yerleştirildi ve para düşürüldü.");
    }

    public void SetTowerToPlace(TowerData towerDataToSet)
    {
        selectedTowerData = towerDataToSet;
        Debug.Log(
            $"{towerDataToSet.towerName} ({towerDataToSet.towerType}) seçildi. Fiyat: {towerDataToSet.cost}"
        );
    }
}
