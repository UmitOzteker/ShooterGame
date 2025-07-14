using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Camera cam;

    void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }

        if (slider == null)
        {
            return;
        }

        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 1f;

        // Kamera debug'ı
        if (cam == null)
        {
            cam = Camera.main;
        }

        // Canvas ve parent kontrolleri
        Canvas canvas = GetComponentInParent<Canvas>();

        // Parent hierarchy kontrolü
        Transform parent = transform.parent;
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        if (slider == null || maxValue <= 0f)
            return;

        float normalizedValue = currentValue / maxValue;
        slider.value = normalizedValue;
    }

    void Update()
    {
        if (cam != null)
        {
            // Rotasyon değişikliğini kontrol et
            Quaternion oldRotation = transform.rotation;

            Vector3 lookDirection = cam.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(lookDirection);

            // Debug için
            Debug.DrawRay(transform.position, lookDirection, Color.red);
        }
    }
}
