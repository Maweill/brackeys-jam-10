using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseTemplate : MonoBehaviour
{
    [SerializeField] public Transform square2; 

    void Update()
    {
        if (square2 == null) return;
        float distance = Vector2.Distance(transform.position, square2.position);
        float maxSize = Mathf.Max(transform.localScale.x, square2.localScale.x);
        float distancePercentage = Mathf.Clamp01(1 - (distance / maxSize));

        // Преобразование расстояния в проценты
        float percentage = distancePercentage * 100f;

        // Выводим значение в процентах в консоль
        Debug.Log("Расстояние в процентах: " + percentage.ToString("F2") + "%");
    }
}