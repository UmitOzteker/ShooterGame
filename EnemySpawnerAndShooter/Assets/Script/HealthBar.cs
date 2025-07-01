using System.Runtime.CompilerServices;
// HealthBar.cs - Debug versiyonu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cam;


    void Start()
    {
        // Slider'ı bul
        if (slider == null)
        {
            slider = GetComponent<Slider>();
            Debug.Log("Slider GetComponent ile bulundu");
        }

        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
            Debug.Log("Slider GetComponentInChildren ile bulundu");
        }

        if (slider == null)
        {
            Debug.LogError("SLIDER BULUNAMADI! Inspector'da slider atayın!");
            return;
        }



        // Slider ayarları
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 1f;

        Debug.Log($"HealthBar başlatıldı. Slider: {slider.name}, Value: {slider.value}");
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        Debug.Log($"UpdateHealthBar çağrıldı: {currentValue}/{maxValue}");

        if (slider == null)
        {
            Debug.LogError("Slider null! UpdateHealthBar başarısız!");
            return;
        }

        if (maxValue <= 0)
        {
            Debug.LogError("MaxValue sıfır veya negatif!");
            return;
        }

        float normalizedValue = currentValue / maxValue;
        slider.value = normalizedValue;

        Debug.Log($"Slider güncellendi: {normalizedValue} (Slider.value: {slider.value})");

        // Görsel kontrol
        if (slider.fillRect != null)
        {
            Debug.Log($"Fill Rect mevcut: {slider.fillRect.name}");
        }
        else
        {
            Debug.LogWarning("Fill Rect bulunamadı!");
        }
    }

    void Update()
    {
        if (cam != null)
        {
            transform.LookAt(Camera.main.transform);
        }

    }
}