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

    [SerializeField]
    private LayerMask towerPlacementLayerMask;

    private Vector3 clickedPosition;
    private Vector2 clickedScreenPosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (currencyScript == null)
            {
                currencyScript = FindObjectOfType<Currency>();
                if (currencyScript == null)
                {
                    return;
                }
            }

            Ray camRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(playerCamera.transform.position, camRay.direction * 100f, Color.red, 2f);

            if (Physics.Raycast(camRay, out RaycastHit hitInfo, 100f, towerPlacementLayerMask))
            {
                clickedPosition = hitInfo.point;
                clickedScreenPosition = Input.mousePosition;
                if (PopupManager.Instance != null)
                {
                    PopupManager.Instance.OpenTowerPopup(clickedScreenPosition);
                }
            }
        }
    }

    public void PlaceTower()
    {
        if (selectedTowerData == null)
        {
            return;
        }
        int currentMoney = currencyScript.GetBloodMoneyAmount();

        if (currentMoney < selectedTowerData.cost)
        {
            return;
        }

        Vector3 spawnPosition = clickedPosition + Vector3.up * 0.5f;
        Instantiate(selectedTowerData.prefab, spawnPosition, Quaternion.identity);
        currencyScript.DecreaseBloodMoneyAmount(selectedTowerData.cost);
    }

    public void SetTowerToPlace(TowerData towerDataToSet)
    {
        selectedTowerData = towerDataToSet;
    }
}
