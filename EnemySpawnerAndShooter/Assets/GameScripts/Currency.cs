using TMPro;
using UnityEngine;

public class Currency : MonoBehaviour
{
    public TextMeshProUGUI bloodMoneyText;
    public int bloodMoneyAmount = 0;

    void Start()
    {
        UpdateUI();
    }

    public void IncreaseBloodMoneyAmount(int amount)
    {
        bloodMoneyAmount += amount;
        UpdateUI();
    }

    public void DecreaseBloodMoneyAmount(int amount)
    {
        bloodMoneyAmount -= amount;
        UpdateUI();
    }

    public int GetBloodMoneyAmount()
    {
        return bloodMoneyAmount;
    }

    private void UpdateUI()
    {
        if (bloodMoneyText != null)
        {
            bloodMoneyText.text = $"{bloodMoneyAmount}";
        }
        else
        {
            Debug.LogError("bloodMoneyText null! Inspector'dan TextMeshPro referansını atayın!");
        }
    }
}
