using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class HouseTemplate : MonoBehaviour
{
    [SerializeField]
    private Transform _house; 

    private void Update()
    {
        if (_house == null) return;
        float distance = Vector2.Distance(transform.position, _house.position);
        float maxSize = Mathf.Max(transform.localScale.x, _house.localScale.x);
        float distancePercentage = Mathf.Clamp01(1 - (distance / maxSize));

        // Преобразование расстояния в проценты
        float percentage = distancePercentage * 100f;

        // Выводим значение в процентах в консоль
        Debug.Log("Расстояние в процентах: " + percentage.ToString("F2") + "%");
    }
}