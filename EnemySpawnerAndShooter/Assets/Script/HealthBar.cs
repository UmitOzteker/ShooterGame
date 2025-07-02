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
            // Kameraya doğru döndür (ama ters çevrilmiş olmaması için fark alınır)
            Vector3 lookDirection = transform.position - cam.transform.position;
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
}
