using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [SerializeField]
    private GameObject towerPopup;

    [SerializeField]
    private TowerData basicTowerData;

    [SerializeField]
    private TowerData fireTowerData;

    [SerializeField]
    private Tower towerScript; // Tower script referansı

    [SerializeField]
    private Button placeButton;

    [SerializeField]
    private Button cancelButton;
    private System.Action onPlaceAction;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        towerPopup.SetActive(false);
        placeButton.onClick.AddListener(OnBasicTowerClicked);
        cancelButton.onClick.AddListener(OnFireTowerClicked);
    }

    public void OpenTowerPopup(Vector2 screenPosition)
    {
        RectTransform popupRect = towerPopup.GetComponent<RectTransform>();
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            popupRect.parent as RectTransform,
            screenPosition,
            null,
            out anchoredPos
        );
        popupRect.anchoredPosition = anchoredPos;
        towerPopup.SetActive(true);
    }

    private void OnBasicTowerClicked()
    {
        towerScript.SetTowerToPlace(basicTowerData);
        towerPopup.SetActive(false);
        towerScript.PlaceTower();
    }

    private void OnFireTowerClicked()
    {
        towerScript.SetTowerToPlace(fireTowerData);
        towerPopup.SetActive(false);
        towerScript.PlaceTower();
    }
}
